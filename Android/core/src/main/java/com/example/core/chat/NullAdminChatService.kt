package com.example.core.chat

import com.domain.chat.ChatMessage
import com.domain.chat.IAdminChatService
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.MutableSharedFlow
import kotlinx.coroutines.flow.MutableStateFlow

class NullAdminChatService: IAdminChatService {
    override suspend fun sendMessageToAdmin(message: String) {

    }

    override suspend fun receiveMessages(): Flow<ChatMessage> {
        return MutableSharedFlow()
    }

    companion object {
        val Instance = NullAdminChatService()
    }
}