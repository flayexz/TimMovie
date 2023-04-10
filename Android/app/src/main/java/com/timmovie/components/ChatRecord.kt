package com.timmovie.components

import androidx.compose.foundation.layout.Row
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.tooling.preview.Preview

@Composable
fun ChatRecord(username: String, content: String) {
    Row {
        Text(text = "$username: ", fontWeight = FontWeight.Bold)
        Text(text = content)
    }
}

@Composable
@Preview
fun SampleChatRecord() {
    ChatRecord(username = "Username", content = "hello, world")
}