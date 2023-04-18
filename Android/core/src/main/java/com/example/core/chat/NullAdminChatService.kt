package com.example.core.chat

import com.domain.chat.ChatMessage
import com.domain.chat.IAdminChatService

class NullAdminChatService: IAdminChatService {
    override suspend fun sendMessageToAdmin(message: String) {

    }

    override fun registerOnMessageReceivedHandler(handler: (ChatMessage) -> Unit) {

    }

    override fun unregisterOnMessageReceivedHandler(handler: (ChatMessage) -> Unit) {
    }

    companion object {
        val Instance = NullAdminChatService()
    }
}