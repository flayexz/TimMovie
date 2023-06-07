package com.timmovie.fragments.chat.chat_admin

import android.widget.Toast
import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import com.example.core.Constants
import com.example.core.MyChannel
import com.example.core.User
import com.google.protobuf.Empty
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState
import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder
import io.grpc.stub.StreamObserver
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel) {
    LaunchedEffect(Unit) {
        try {
            viewModel.getChatMessages()
        } catch(_: Exception) {
        }
        try {
            viewModel.getEvents()
        } catch(_: Exception) {
        }
    }
    ChatAdminPageInternal(
        records = viewModel.records,
        onExitClick =  {

            val grpcService = ChatGrpc.newStub(MyChannel)
            val request = ChatOuterClass.AttachedClient.newBuilder()
                .setName(User.name)
                .build()
            GlobalScope.launch(Dispatchers.IO) {
                grpcService.disconnectUserFromChat(request, object : StreamObserver<Empty> {
                    override fun onNext(response: Empty) {
                        // Обработка ответа сервера
                    }

                    override fun onError(t: Throwable) {
                        // Обработка ошибки
                    }

                    override fun onCompleted() {
                        // Завершение операции
                        viewModel.machine.currentState.value = AppState.Login
                    }
                })
            }
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
