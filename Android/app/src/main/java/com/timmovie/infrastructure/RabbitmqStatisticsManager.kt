package com.timmovie.infrastructure

import android.util.Log
import com.google.gson.Gson
import com.google.gson.reflect.TypeToken
import com.rabbitmq.client.AMQP
import com.rabbitmq.client.Connection
import com.rabbitmq.client.ConnectionFactory
import com.rabbitmq.client.DefaultConsumer
import com.rabbitmq.client.Envelope
import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.components.SingletonComponent
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.ExecutorCoroutineDispatcher
import kotlinx.coroutines.channels.awaitClose
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.callbackFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.yield
import kotlin.coroutines.resume
import kotlin.coroutines.suspendCoroutine

class RabbitmqStatisticsManager(private val connectionFactory: ConnectionFactory,
                                private val exchangeName: String,
                                private val routingKey: String = ""): StatisticsManager {


    override suspend fun getFilmStatistics(): Flow<Map<String, Int>> {
        return suspendCoroutine {
            Log.i(LogTag, "Создаю соединение")
            val connection = connectionFactory.newConnection()
            Log.i(LogTag, "Создаю канал")
            val channel = connection.createChannel()
            Log.i(LogTag, "Канал создан. Декларирую обменник")
            channel.exchangeDeclare(exchangeName, "fanout")
            Log.i(LogTag, "Обменник создан. Создаю очередь")
            val queue = channel.queueDeclare()
            Log.i(LogTag, "Очередь создана. Связываю с обменником")
            channel.queueBind(queue.queue, exchangeName, routingKey)
            Log.i(LogTag, "Очередь связана. Подписываюсь на события")
            it.resume(callbackFlow {
                val consumerTag = channel.basicConsume(queue.queue, true, object: DefaultConsumer(channel) {
                    override fun handleDelivery(
                        consumerTag: String?,
                        envelope: Envelope?,
                        properties: AMQP.BasicProperties?,
                        body: ByteArray?
                    ) {
                        if (body != null) {
                            try {
                                trySend(deserializeStatisticsRecord(body))
                            } catch(e: Exception) {
                                Log.e(LogTag, "Ошибка десериализации сообщения из рэббита")
                            }
                        }
                        super.handleDelivery(consumerTag, envelope, properties, body)
                    }
                })

                Log.d(LogTag, "Зарегистрирован обработчик. Присвоен тэг $consumerTag")

                awaitClose {
                    Log.i(LogTag, "Отменяю подписку")
                    channel.basicCancel(consumerTag)
                    Log.i(LogTag, "Подписка отменена")
                }
            })
        }
    }

    companion object {
        val LogTag = RabbitmqStatisticsManager::class.simpleName
        val gson = Gson()
        private val mapType = object: TypeToken<Map<String, Int>>(){}.type
        fun deserializeStatisticsRecord(payload: ByteArray): Map<String, Int> {
            val str = payload.decodeToString()
            val map = gson.fromJson<Map<String, Int>>(str, mapType)
            return map
        }
    }
}

@Module
@InstallIn(SingletonComponent::class)
object RabbitmqStatisticsManagerProvider {
    val connectionFactory = ConnectionFactory().apply {
        setUri("amqp://10.0.2.2:5672")
        virtualHost = "/"
        username = "guest"
        password = "guest"
    }

    @Provides
    fun provideStatisticsManager(): StatisticsManager {
        return RabbitmqStatisticsManager(connectionFactory, "statistics-exchange", "")
    }
}

interface StatisticsManager {
    suspend fun getFilmStatistics(): Flow<Map<String, Int>>
}