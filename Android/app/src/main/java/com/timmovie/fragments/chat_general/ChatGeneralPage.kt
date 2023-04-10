package com.timmovie.fragments.chat_general

import android.annotation.SuppressLint
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.lazy.rememberLazyListState
import androidx.compose.material.*
import androidx.compose.runtime.*
import androidx.compose.runtime.setValue
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.constraintlayout.compose.ConstraintLayout
import androidx.constraintlayout.compose.Dimension

import com.timmovie.components.ChatRecord
import com.timmovie.components.InputText

@Composable
fun ChatGeneralPage(records: MutableList<ChatRecordItem>) {
    Scaffold(topBar = {
        TopAppBar() {
            Text(text = "Что то")
        }
    }, bottomBar = {}) {
        it.calculateTopPadding()
        var inputMessage by remember {
            mutableStateOf("")
        }
        Column(modifier = Modifier.fillMaxHeight()) {
            LazyColumn(modifier = Modifier.weight(1f).padding(5.dp)) {
                items(records) {
                    ChatRecord(username = it.username, content = it.content)
                }
            }
            Box() {
                InputText(
                    text = inputMessage,
                    onValueChange = {
                        inputMessage = it
                    },
                    placeholder = {
                        Text("Введите сообщение")
                    },
                    modifier = Modifier.fillMaxWidth())
            }
            Box(modifier = Modifier.fillMaxWidth()) {
                Row(horizontalArrangement = Arrangement.SpaceBetween,
                    verticalAlignment = Alignment.CenterVertically,
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Button(
                        onClick = {
                            records.add(ChatRecordItem("asdfa", "asdfsdf"))
                            inputMessage = ""
                        },
                        modifier = Modifier
                            .weight(1f)
                            .padding(end = 5.dp)) {
                        Text(text = "Отправить")
                    }
                    Button(onClick = { /*TODO*/ }) {
                        Text(text = "П")
                    }
                }
            }
        }

    }
}

@SuppressLint("UnusedMaterialScaffoldPaddingParameter")
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

    Scaffold() {
        ChatGeneralPage(records = records)
    }
}

