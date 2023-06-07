package com.timmovie.fragments.chat.chat_admin

import ChatGrpc
import ChatOuterClass
import android.app.AlertDialog
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.State
import androidx.compose.runtime.livedata.observeAsState
import androidx.compose.ui.platform.LocalContext
import com.google.protobuf.Empty
import com.timmovie.MainActivity
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState
import com.timmovie.theme.MyChannel
import com.timmovie.theme.User
import com.timmovie.theme.isBuilded
import io.grpc.stub.StreamObserver
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.util.logging.Logger

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel) {
    val myRecords = viewModel.records.observeAsState()
    
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

    if (!isBuilded) {
        val builder = AlertDialog.Builder(LocalContext.current)
        builder.setTitle("Confirmation")
        builder.setMessage("Are you sure?")
        builder.setPositiveButton("Да") { _, _ ->
            try {
                viewModel.getChatMessages()
                try {
                    Thread.sleep(500)
                    viewModel.getEvents()
                    isBuilded = true;
                } catch (e: Exception) {
                    val Log = Logger.getLogger(MainActivity::class.java.name)
                    Log.warning(e.message)
                }
            } catch (e: Exception) {
                val Log = Logger.getLogger(MainActivity::class.java.name)
                Log.warning(e.message)
            }
        }
        builder.setNegativeButton("Нет") { _, _ ->
            viewModel.machine.currentState.value = AppState.Login
        }
        builder.create().show()
    }
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