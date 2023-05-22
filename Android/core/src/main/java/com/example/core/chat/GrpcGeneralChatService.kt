package com.example.core.chat

import com.domain.chat.ChatMessage
import com.domain.chat.IGeneralChatService
import com.example.grpc.GeneralChatServiceGrpcKt
import com.example.grpc.sendMessageRequest
import com.google.protobuf.Empty
import io.grpc.Channel
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.transform

class GrpcGeneralChatService(channel: Channel): IGeneralChatService {
    private val stub = GeneralChatServiceGrpcKt.GeneralChatServiceCoroutineStub(channel)
    override suspend fun sendMessage(message: String) {
        stub.sendMessage(sendMessageRequest {
            this.message = message
        })
    }

    override suspend fun receiveMessages(): Flow<ChatMessage> {
        return stub.getMessagesStream(Empty.getDefaultInstance())
            .transform {
                ChatMessage(
                    username = it.username,
                    message = it.message
                )
            }
    }
}