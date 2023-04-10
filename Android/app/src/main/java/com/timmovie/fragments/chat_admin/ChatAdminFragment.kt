package com.timmovie.fragments.chat_admin

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
import com.timmovie.components.InputText
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
