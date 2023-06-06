package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import com.example.core.Constants
import com.example.core.User
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel) {
    ChatAdminPageInternal(
        records = viewModel.records,
        onExitClick =  {

            val channel: ManagedChannel = ManagedChannelBuilder
                .forAddress(Constants.Urls.HOST, Constants.Urls.PORT)
                .usePlaintext()
                .build()
            val grpcService = ChatGrpc.newBlockingStub(channel)
            val request = ChatOuterClass.AttachedClient.newBuilder()
                .setName(User.name)
                .build()
            grpcService.disconnectUserFromChat(request)

            viewModel.machine.currentState.value = AppState.Login
        },
        message = viewModel.message,
        onMessageButtonClick = {
            viewModel.sendMessage()
        },
        onMessageChange = {
            viewModel.message = it
        }
    )
}

@Composable
fun ChatAdminPageInternal(
    records: MutableList<ChatRecordItem>,
    onExitClick: () -> Unit,
    message: String,
    onMessageButtonClick: () -> Unit,
    onMessageChange: (String) -> Unit
) {
    ChatPageBase(
        records = records,
        onExitClick = onExitClick,
        inputMessage = message,
        onMessageSendClick = onMessageButtonClick,
        onInputMessageChange = onMessageChange
    )
}
@Preview
@Composable
fun ChatAdminPagePreview() {
    val records = remember {
        mutableStateListOf(
            ChatRecordItem("Вы", "Дорова епта"),
            ChatRecordItem("Пахан", "Че надо?"),
        )
    }
    ChatAdminPageInternal(records = records, {}, "", {}, {})
}
