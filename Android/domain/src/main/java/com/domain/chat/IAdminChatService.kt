package com.domain.chat

import kotlinx.coroutines.flow.Flow

interface IAdminChatService {
    suspend fun sendMessageToAdmin(message: String)
    suspend fun receiveMessages(): Flow<ChatMessage>
}