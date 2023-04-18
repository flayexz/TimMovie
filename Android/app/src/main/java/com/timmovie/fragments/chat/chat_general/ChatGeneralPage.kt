package com.timmovie.fragments.chat.chat_general

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview

import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState

@Composable
fun ChatGeneralPage(viewModel: ChatGeneralViewModel, onToGeneralPageClick: () -> Unit) {
    ChatGeneralPageInternal(
        records = viewModel.records,
        onAnotherPageClick =  onToGeneralPageClick,
        onExitClick =  {
            viewModel.machine.currentState.value = AppState.Login
        },
        onMessageChange = {
            viewModel.message = it
        },
        onMessageButtonClick = {
            viewModel.sendMessage()
        },
        message = viewModel.message
    )
}

@Composable
fun ChatGeneralPageInternal(
    records: MutableList<ChatRecordItem>,
    onAnotherPageClick: () -> Unit,
    onExitClick: () -> Unit,
    message: String,
    onMessageButtonClick: () -> Unit,
    onMessageChange: (String) -> Unit
) {
    ChatPageBase(
        records = records,
        onAnotherPageClick = onAnotherPageClick,
        anotherPageName = "К пахану",
        onExitClick = onExitClick,
        inputMessage = message,
        onInputMessageChange = onMessageChange,
        onMessageSendClick = onMessageButtonClick
    )
}

@Preview
@Composable
fun ChatGeneralPagePreview() {
    val records = remember {
        mutableStateListOf(
            ChatRecordItem("user", "content"),
            ChatRecordItem("user2", "привет всем, сегодня работаем в 2 раза интенсивнее"),
            ChatRecordItem("user3", "а че всмысле? зачем?"),
            ChatRecordItem("user4", "работать негры!"),
        )
    }

    ChatGeneralPageInternal(
        records = records,
        onExitClick =  {},
        onAnotherPageClick = {},
        message = "",
        onMessageButtonClick = {},
        onMessageChange = {}
    )
}

