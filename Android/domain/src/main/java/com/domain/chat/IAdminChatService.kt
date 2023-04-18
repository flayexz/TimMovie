package com.domain.chat

interface IAdminChatService {
    suspend fun sendMessageToAdmin(message: String)
    fun registerOnMessageReceivedHandler(handler: (ChatMessage) -> Unit)
    fun unregisterOnMessageReceivedHandler(handler: (ChatMessage) -> Unit)
}