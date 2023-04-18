package com.example.core.chat

import com.domain.chat.ChatMessage
import com.domain.chat.IGeneralChatService

class NullGeneralChatService: IGeneralChatService {
    override suspend fun sendMessage(message: String) {

    }

    override fun registerOnMessageReceivedHandler(handler: (ChatMessage) -> Unit) {
    }

    override fun unregisterOnMessageReceivedHandler(handler: (ChatMessage) -> Unit) {
    }

    companion object {
        val Instance = NullGeneralChatService()
    }
}