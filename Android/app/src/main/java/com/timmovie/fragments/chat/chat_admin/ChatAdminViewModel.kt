package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.domain.chat.ChatMessage
import com.domain.chat.IAdminChatService
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import kotlinx.coroutines.launch

class ChatAdminViewModel(val machine: AppStateMachine, val service: IAdminChatService) : ViewModel() {
    private val onMessageReceivedHandler: (ChatMessage) -> Unit = {
        records.add(ChatRecordItem(it.username, it.message))
    }

    init {
        service.registerOnMessageReceivedHandler(handler = onMessageReceivedHandler)
    }

    override fun onCleared() {
        super.onCleared()
        service.unregisterOnMessageReceivedHandler(onMessageReceivedHandler)
    }

    val records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")
    fun sendMessage() {
        if (message.isEmpty()) return
        viewModelScope.launch {
            service.sendMessageToAdmin(message)
            message = ""
        }
    }
}