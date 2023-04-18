package com.timmovie.fragments.chat.chat_general

import androidx.compose.runtime.mutableStateListOf
import androidx.lifecycle.ViewModel
import com.timmovie.components.ChatRecordItem
import com.timmovie.infrastructure.AppStateMachine

class ChatGeneralViewModel(val machine: AppStateMachine): ViewModel() {
    var records: MutableList<ChatRecordItem> = mutableStateListOf()

}