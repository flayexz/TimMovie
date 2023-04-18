package com.timmovie.fragments.chat

import android.widget.Toast
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.Button
import androidx.compose.material.Scaffold
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.timmovie.components.ChatRecord
import com.timmovie.components.ChatRecordItem
import com.timmovie.components.InputText


@Composable
fun ChatPageBase(records: MutableList<ChatRecordItem>,
                 onExitClick: () -> Unit = {},
                 anotherPageName: String = "",
                 onAnotherPageClick: () -> Unit = {}) {
    Scaffold(topBar = {
        TopAppBar(modifier = Modifier.fillMaxWidth()) {
            Row(horizontalArrangement = Arrangement.SpaceBetween, modifier = Modifier.fillMaxWidth()) {
                Text(text = anotherPageName, fontSize = 16.sp, modifier = Modifier.clickable {
                    onAnotherPageClick()
                })
                Text(text = "Выход", fontSize = 16.sp, modifier = Modifier.clickable {
                    onExitClick()
                })
            }
        }
    }, bottomBar = {}) {
        it.calculateTopPadding()
        var inputMessage by remember {
            mutableStateOf("")
        }
        Column(modifier = Modifier.fillMaxHeight().padding(5.dp)) {
            LazyColumn(modifier = Modifier
                .weight(1f)) {
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

@Preview
@Composable
fun ChatPageBasePreview() {
    val records = remember {
        mutableStateListOf(
            ChatRecordItem("Кто-то", "Привет всем"),
            ChatRecordItem("Это", "ДДААВАЫАЫВАВЫА")
        )
    }
    ChatPageBase(records = records, onExitClick = {}, anotherPageName = "Другая страница", onAnotherPageClick = {})
}
