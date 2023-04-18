package com.timmovie.fragments.chat.chat_general

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.domain.chat.ChatMessage
import com.domain.chat.IGeneralChatService
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import kotlinx.coroutines.launch

class ChatGeneralViewModel(val machine: AppStateMachine, val service: IGeneralChatService): ViewModel() {
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

    var records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")

    fun sendMessage() {
        if (message.isEmpty()) return

        viewModelScope.launch {
            service.sendMessage(message)
            message = ""
        }
    }

}