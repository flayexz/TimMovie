package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.core.Constants
import com.example.core.User
import com.timmovie.MainActivity
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import javax.inject.Inject
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder
import java.util.logging.Logger

@HiltViewModel
class ChatAdminViewModel @Inject constructor(val machine: AppStateMachine) : ViewModel() {
    val records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")
    

    fun sendMessage() {
        if (message.isEmpty()) return
        viewModelScope.launch {
            val channel: ManagedChannel = ManagedChannelBuilder
                .forAddress(Constants.Urls.HOST, Constants.Urls.PORT)
                .usePlaintext()
                .build()
            val service = ChatGrpc.newBlockingStub(channel)
            val request = ChatOuterClass.ChatMessage.newBuilder()
                .setName(User.name)
                .setBody(message)
                .build()
            service.sendMessage(request)
            val Log = Logger.getLogger(MainActivity::class.java.name)
            Log.warning("ОТПРАВИЛ СООБЩЕНИЕ")
            Log.warning(User.name)
            Log.warning(message)

            records.add(ChatRecordItem(User.name, message))
            message = ""
        }
    }

    init {
        viewModelScope.launch {
            
            val channel: ManagedChannel = ManagedChannelBuilder
                .forAddress(Constants.Urls.HOST, Constants.Urls.PORT)
                .usePlaintext()
                .build()
            val grpcService = ChatGrpc.newBlockingStub(channel)
            val requestConnectToChat = ChatOuterClass.AttachedClient.newBuilder()
                .setName(User.name)
                .build()
            val responseConnectUserToChat = grpcService.connectUserToChat(requestConnectToChat)
            val Log = Logger.getLogger(MainActivity::class.java.name)
            Log.warning("ПРИСОЕДИНИЛСЯ К ЧАТУ")

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

            val requestConnectToEvents = ChatOuterClass.AttachedClient.newBuilder()
                .setName(User.name)
                .build()
            val responseConnectToEvents = grpcService.connectToEvents(requestConnectToEvents)
            responseConnectToEvents.forEach {
                records.add(ChatRecordItem(
                    username = "notification",
                    content = it.body
                ))
                val Log = Logger.getLogger(MainActivity::class.java.name)
                Log.warning("ДОБАВЛЯЕМ В ЧАТ")
                Log.warning("notification")
                Log.warning(it.body)
            }
        }
    }
}