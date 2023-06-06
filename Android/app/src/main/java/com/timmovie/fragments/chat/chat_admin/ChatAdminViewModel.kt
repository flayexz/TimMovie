package com.timmovie.fragments.chat.chat_admin

import android.content.SharedPreferences
import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.domain.chat.IAdminChatService
import com.timmovie.MainActivity
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.flow.collect
import kotlinx.coroutines.launch
import java.util.logging.Logger
import javax.inject.Inject

@HiltViewModel
class ChatAdminViewModel @Inject constructor(val machine: AppStateMachine,
                                             val service: IAdminChatService) : ViewModel() {
//                                             private val sharedPreferences: SharedPreferences) : ViewModel() {
    val records: MutableList<ChatRecordItem> = mutableStateListOf()
    var message by mutableStateOf("")

    fun sendMessage() {
        if (message.isEmpty()) return
        viewModelScope.launch {
            val Log = Logger.getLogger(MainActivity::class.java.name)
//            Log.warning(sharedPreferences.getString("login", ""))
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