package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.LiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.domain.chat.ChatMessage
import com.domain.chat.IAdminChatService
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class ChatAdminViewModel @Inject constructor(val machine: AppStateMachine,
                                             val service: IAdminChatService) : ViewModel() {
    val records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")

    fun sendMessage() {
        if (message.isEmpty()) return
        viewModelScope.launch {
            service.sendMessageToAdmin(message)
            message = ""
        }
    }

    init {
        viewModelScope.launch {
            launch {
                service.receiveMessages().collect {
                    records.add(ChatRecordItem(
                        username = it.username,
                        content = it.message
                    ))
                }
            }
        }
    }
}