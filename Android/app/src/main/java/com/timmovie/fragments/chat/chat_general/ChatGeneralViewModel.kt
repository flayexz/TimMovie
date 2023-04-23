package com.timmovie.fragments.chat.chat_general

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.domain.chat.IGeneralChatService
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class ChatGeneralViewModel @Inject constructor(val machine: AppStateMachine,
                                               val service: IGeneralChatService): ViewModel() {
    var records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")

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

    fun sendMessage() {
        if (message.isEmpty()) return

        viewModelScope.launch {
            service.sendMessage(message)
            message = ""
        }
    }
}