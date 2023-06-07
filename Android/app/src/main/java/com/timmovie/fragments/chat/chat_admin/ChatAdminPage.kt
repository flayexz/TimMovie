package com.timmovie.fragments.chat.chat_admin

import ChatGrpc
import ChatOuterClass
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.State
import androidx.compose.runtime.livedata.observeAsState
import com.google.protobuf.Empty
import com.timmovie.MainActivity
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState
import com.timmovie.theme.MyChannel
import com.timmovie.theme.User
import io.grpc.stub.StreamObserver
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.util.logging.Logger

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel) {
    val myRecords = viewModel.records.observeAsState()
    LaunchedEffect(Unit) {
        try {
            viewModel.getChatMessages()
            try {
                withContext(Dispatchers.IO) {
                    Thread.sleep(1000)
                    viewModel.getEvents()
                }
            } catch(e: Exception) {
                val Log = Logger.getLogger(MainActivity::class.java.name)
                Log.warning(e.message)
            }
        } catch(e: Exception) {
            val Log = Logger.getLogger(MainActivity::class.java.name)
            Log.warning(e.message)
        }
    }

    ChatAdminPageInternal(
        records = myRecords,
        onExitClick =  {

            val grpcService = ChatGrpc.newStub(MyChannel)
            val request = ChatOuterClass.AttachedClient.newBuilder()
                .setName(User.name)
                .build()
            viewModel.machine.currentState.value = AppState.Login
            grpcService.disconnectUserFromChat(request, object : StreamObserver<Empty> {
                override fun onNext(response: Empty) {
                    // Обработка ответа сервера
                }

                override fun onError(t: Throwable) {
                    // Обработка ошибки
                }

                override fun onCompleted() {
                    // Завершение операции
                }
            })
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
    records: State<List<ChatRecordItem>?>,
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