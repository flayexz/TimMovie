package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel) {
    ChatAdminPageInternal(
        records = viewModel.records,
        onExitClick =  {
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
