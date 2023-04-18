package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase
import com.timmovie.infrastructure.AppState

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel, onToGeneralChatClick: () -> Unit) {
    ChatAdminPageInternal(
        records = viewModel.records,
        onExitClick =  {
            viewModel.machine.currentState.value = AppState.Login
        },
        onAnotherPageClick = onToGeneralChatClick
    )
}

@Composable
fun ChatAdminPageInternal(
    records: MutableList<ChatRecordItem>,
    onAnotherPageClick: () -> Unit,
    onExitClick: () -> Unit
) {
    ChatPageBase(
        records = records,
        anotherPageName = "К пацанам",
        onAnotherPageClick = onAnotherPageClick,
        onExitClick = onExitClick
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
    ChatAdminPageInternal(records = records, {}, {})
}
