package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.mutableStateListOf
import androidx.lifecycle.ViewModel
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine

class ChatAdminViewModel(val machine: AppStateMachine) : ViewModel() {
    val records: MutableList<ChatRecordItem> = mutableStateListOf()
}