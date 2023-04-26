package com.example.core.chat

import com.domain.chat.ChatMessage
import com.domain.chat.IGeneralChatService
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.flow

class NullGeneralChatService: IGeneralChatService {
    override suspend fun sendMessage(message: String) {

    }

    override suspend fun receiveMessages(): Flow<ChatMessage> {
        return flow {  }
    }

    companion object {
        val Instance = NullGeneralChatService()
    }
}