package com.timmovie.fragments.chat.chat_general

import androidx.compose.runtime.*
import androidx.compose.ui.tooling.preview.Preview

import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPageBase

@Composable
fun ChatGeneralPage(viewModel: ChatGeneralViewModel) {
    ChatGeneralPageInternal(records = viewModel.records)
}

@Composable
fun ChatGeneralPageInternal(records: MutableList<ChatRecordItem>) {
    ChatPageBase(records = records)
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

    ChatGeneralPageInternal(records = records)
}

