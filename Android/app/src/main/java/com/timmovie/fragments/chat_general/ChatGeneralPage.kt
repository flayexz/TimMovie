package com.timmovie.fragments.chat_general

import android.annotation.SuppressLint
import android.widget.Toast
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.*
import androidx.compose.runtime.*
import androidx.compose.runtime.setValue
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

import com.timmovie.components.ChatRecord
import com.timmovie.components.ChatRecordItem
import com.timmovie.components.InputText
import com.timmovie.fragments.ChatPageBase

@Composable
fun ChatGeneralPage(records: MutableList<ChatRecordItem>) {
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

    ChatGeneralPage(records = records)
}

