package com.timmovie.fragments.chat_admin

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.ChatPageBase

@Composable
fun ChatAdminPage(records: MutableList<ChatRecordItem>) {
    ChatPageBase(records = records)
}

@Preview
@Composable
fun ChatAdminPagePreview() {
    val records = remember {
        mutableStateListOf(ChatRecordItem("Вы", "Дорова епта"),
        ChatRecordItem("Пахан", "Че надо?")
        )
    }
    ChatAdminPage(records = records)
}
