package com.domain.chat

import kotlinx.coroutines.flow.Flow


interface IGeneralChatService {
    suspend fun sendMessage(message: String)
    suspend fun receiveMessages(): Flow<ChatMessage>
}