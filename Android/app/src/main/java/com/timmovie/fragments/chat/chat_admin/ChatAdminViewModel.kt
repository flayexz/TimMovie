package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.core.Constants
import com.example.core.MyChannel
import com.example.core.User
import com.google.protobuf.Empty
import com.timmovie.MainActivity
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import javax.inject.Inject
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder
import io.grpc.stub.StreamObserver
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.channels.awaitClose
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.callbackFlow
import kotlinx.coroutines.flow.collect
import java.util.logging.Logger

@HiltViewModel
class ChatAdminViewModel @Inject constructor(val machine: AppStateMachine) : ViewModel() {
    val records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")
    

    fun sendMessage() {
        if (message.isEmpty()) return
        viewModelScope.launch {

            val grpcService = ChatGrpc.newStub(MyChannel)
            val request = ChatOuterClass.ChatMessage.newBuilder()
                .setName(User.name)
                .setBody(message)
                .build()
            GlobalScope.launch(Dispatchers.IO) {
                grpcService.sendMessage(request, object : StreamObserver<Empty> {
                    override fun onNext(response: Empty) {
                        // Обработка ответа сервера
                        val Log = Logger.getLogger(MainActivity::class.java.name)
                        Log.warning("Обработка ответа сервера")
                    }

                    override fun onError(t: Throwable) {
                        // Обработка ошибки
                        val Log = Logger.getLogger(MainActivity::class.java.name)
                        Log.warning(t.message)
                    }

                    override fun onCompleted() {
                        // Завершение операции
                        val Log = Logger.getLogger(MainActivity::class.java.name)
                        Log.warning("ОТПРАВИЛ СООБЩЕНИЕ")
                        Log.warning(User.name)
                        Log.warning(message)

                        records.add(ChatRecordItem(User.name, message))
                        message = ""
                    }
                })
            }
        }
    }

//    init {
//        viewModelScope.launch {
//            launch {
//                getChatMessages()
//                    .collect{ }
//            }
//            
//            launch {
//                getEvents()
//                    .collect{ }
//            }
//        }
//    }

    suspend fun getChatMessages(): Flow<ChatOuterClass.ChatMessage>{
        val grpcService = ChatGrpc.newBlockingStub(MyChannel)
        val requestConnectToChat = ChatOuterClass.AttachedClient.newBuilder()
            .setName(User.name)
            .build()
        val responseConnectUserToChat = grpcService.connectUserToChat(requestConnectToChat)
        val Log = Logger.getLogger(MainActivity::class.java.name)
        Log.warning("ПРИСОЕДИНИЛСЯ К ЧАТУ")
        return callbackFlow {
            responseConnectUserToChat.forEach {
                records.add(ChatRecordItem(
                    username = it.name,
                    content = it.body
                ))
                val Log = Logger.getLogger(MainActivity::class.java.name)
                Log.warning("ДОБАВЛЯЕМ В ЧАТ")
                Log.warning(it.name)
                Log.warning(it.body)
            }

            awaitClose {

            }
        }
    }

    suspend fun getEvents(): Flow<ChatOuterClass.ChatEvent>{
        val requestConnectToEvents = ChatOuterClass.AttachedClient.newBuilder()
            .setName(User.name)
            .build()
        val grpcService = ChatGrpc.newBlockingStub(MyChannel)
        val responseConnectToEvents = grpcService.connectToEvents(requestConnectToEvents)

        return callbackFlow {
            responseConnectToEvents.forEach {
                records.add(
                    ChatRecordItem(
                        username = "notification",
                        content = it.body
                    )
                )
                val Log = Logger.getLogger(MainActivity::class.java.name)
                Log.warning("ДОБАВЛЯЕМ В ЧАТ")
                Log.warning("notification")
                Log.warning(it.body)
            }

            awaitClose {

            }
        }
    }
}