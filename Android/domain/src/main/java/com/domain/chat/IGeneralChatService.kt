package com.domain.chat

interface IGeneralChatService {
    suspend fun sendMessage(message: String)
    fun registerOnMessageReceivedHandler(handler: (ChatMessage) -> Unit)
    fun unregisterOnMessageReceivedHandler(handler: (ChatMessage) -> Unit)
}