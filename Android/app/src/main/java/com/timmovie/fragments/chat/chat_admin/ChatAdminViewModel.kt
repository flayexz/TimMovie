package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.core.*
import com.google.protobuf.Empty
import com.timmovie.MainActivity
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import com.timmovie.theme.MyGrpcService
import com.timmovie.theme.User
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import javax.inject.Inject
import io.grpc.stub.StreamObserver
import java.util.logging.Logger

@HiltViewModel
class ChatAdminViewModel @Inject constructor(val machine: AppStateMachine) : ViewModel() {
    val records = MutableLiveData<List<ChatRecordItem>>()
    var message by mutableStateOf("")


    fun sendMessage() {
        if (message.isEmpty()) return
            val request = ChatOuterClass.ChatMessage.newBuilder()
                .setName(User.name)
                .setBody(message)
                .build()
            MyGrpcService.chatStub.sendMessage(request, object : StreamObserver<Empty> {
                override fun onNext(response: Empty) {
                    // Обработка ответа сервера

                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning("ОТПРАВИЛ СООБЩЕНИЕ")
                    Log.warning(User.name)
                    Log.warning(message)
                }

                override fun onError(t: Throwable) {
                    // Обработка ошибки
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning(t.message)
                }

                override fun onCompleted() {
                    // Завершение операции
                    
                }
            })
        message = ""
    }

    fun getChatMessages() {
        val requestConnectToChat = ChatOuterClass.AttachedClient.newBuilder()
            .setName(User.name)
            .build()
        MyGrpcService.chatStub.connectUserToChat(
            requestConnectToChat,
            object : StreamObserver<ChatOuterClass.ChatMessage> {
                override fun onNext(response: ChatOuterClass.ChatMessage) {
                    // Обработка ответа сервера
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning("Обработка ответа сервера")
                    if (response.name == "init")
                        return

                    val newRecords: List<ChatRecordItem> =
                        listOf(ChatRecordItem(response.name, response.body))

                    val currentRecords: MutableList<ChatRecordItem>? =
                        records.value?.toMutableList()
                    viewModelScope.launch {
                        if (currentRecords == null) {
                            records.value = newRecords
                        } else {
                            currentRecords.addAll(newRecords)
                            records.value = currentRecords!!
                        }
                    }

                    Log.warning(response.name)
                    Log.warning(response.body)
                }

                override fun onError(t: Throwable) {
                    // Обработка ошибки
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning("Обработка ошибки getChatMessage")
                    Log.warning(t.message)
                }

                override fun onCompleted() {
                    // Завершение операции
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                }
            })
    }

    fun getEvents() {
        val requestConnectToEvents = ChatOuterClass.AttachedClient.newBuilder()
            .setName(User.name)
            .build()
        MyGrpcService.chatStub.connectToEvents(
            requestConnectToEvents,
            object : StreamObserver<ChatOuterClass.ChatEvent> {
                override fun onNext(response: ChatOuterClass.ChatEvent) {
                    // Обработка ответа сервера
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning("Обработка ответа сервера")

                    val newRecords: List<ChatRecordItem> =
                        listOf(ChatRecordItem("notification", response.body))

                    val currentRecords: MutableList<ChatRecordItem>? =
                        records.value?.toMutableList()
                    viewModelScope.launch {
                        if (currentRecords == null) {
                            records.value = newRecords
                        } else {
                            currentRecords.addAll(newRecords)
                            records.value = currentRecords!!
                        }
                    }

                    Log.warning("notification")
                    Log.warning(response.body)
                }

                override fun onError(t: Throwable) {
                    // Обработка ошибки
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning("Обработка ошибки events")
                    Log.warning(t.message)
                }

                override fun onCompleted() {
                }
            })
    }
}