package com.example.core.chat

import com.domain.chat.ChatMessage
import com.domain.chat.IAdminChatService
import com.example.grpc.AdminChatServiceGrpc
import com.example.grpc.AdminChatServiceGrpcKt
import com.example.grpc.sendMessageRequest
import com.google.protobuf.Empty
import io.grpc.Channel
import io.grpc.ManagedChannelBuilder
import kotlinx.coroutines.flow.Flow
import kotlinx.coroutines.flow.transform

class GrpcAdminChatService(channel: Channel): IAdminChatService {
    private val stub: AdminChatServiceGrpcKt.AdminChatServiceCoroutineStub

    init {
        stub = AdminChatServiceGrpcKt.AdminChatServiceCoroutineStub(channel)
    }

    override suspend fun sendMessageToAdmin(message: String) {
        stub.sendMessage(sendMessageRequest {
            this.message = message
        })
    }

    override suspend fun receiveMessages(): Flow<ChatMessage> {
        val response = stub.getMessagesStream(Empty.getDefaultInstance())
        return response.transform {
            ChatMessage(
                username = it.username,
                message = it.message
            )
        }
    }
}