package com.timmovie.fragments.login.components

import android.widget.EditText
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Text
import androidx.compose.material.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.MutableState
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.Outline
import androidx.compose.ui.graphics.RectangleShape
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.lifecycle.MutableLiveData
import com.timmovie.theme.Gray

@Composable
fun InputText(text: String,
              onValueChange: (String) -> Unit,
              placeholder: @Composable (() -> Unit)? = null,
              modifier: Modifier = Modifier) {
    TextField(
        value = text,
        placeholder = placeholder,
        onValueChange = onValueChange,
        modifier = modifier
            .background(color = Gray, shape = RoundedCornerShape(6.dp))
            .border(width = 1.dp, color = Color.Black, shape = RoundedCornerShape(6.dp))

    )
}

@Preview("Без введенного текста")
@Composable
fun InputTextPreviewTextLess() {
    var data by remember {
        mutableStateOf("")
    }
    InputText(
        text = data,
        placeholder = {
            Text(text = "Плейсхолдер")
        },
        onValueChange = {
            data = it
        })
}


@Preview("Текст введен")
@Composable
fun InputTextPreviewWithText() {
    val data by remember {
        mutableStateOf("Текст введен")
    };
    InputText(text = data, placeholder = {
        Text(text = "Не должен показаться")
    }, onValueChange = {

    })
}