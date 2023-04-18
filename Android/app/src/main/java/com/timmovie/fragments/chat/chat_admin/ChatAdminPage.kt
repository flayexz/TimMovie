package com.timmovie.fragments.chat.chat_admin

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase

@Composable
fun ChatAdminPage(viewModel: ChatAdminViewModel) {
    ChatAdminPageInternal(records = viewModel.records)
}

@Composable
fun ChatAdminPageInternal(records: MutableList<ChatRecordItem>) {
    ChatPageBase(records = records)
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
    ChatAdminPageInternal(records = records)
}
