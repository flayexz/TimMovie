package com.timmovie.components

import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Text
import androidx.compose.material.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.compose.ui.text.input.VisualTransformation
import com.timmovie.theme.Gray

@Composable
fun InputText(
    text: String,
    onValueChange: (String) -> Unit,
    modifier: Modifier = Modifier,
    visualTransformation: VisualTransformation = VisualTransformation.None,
    placeholder: @Composable() (() -> Unit)? = null,
) {
    TextField(
        value = text,
        placeholder = placeholder,
        onValueChange = onValueChange,
        modifier = modifier
            .background(color = Gray, shape = RoundedCornerShape(6.dp))
            .border(width = 1.dp, color = Color.Black, shape = RoundedCornerShape(6.dp)),
        visualTransformation = visualTransformation
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
        onValueChange = {
            data = it
        }) {
            Text(text = "Плейсхолдер")
        }
}


@Preview("Текст введен")
@Composable
fun InputTextPreviewWithText() {
    val data by remember {
        mutableStateOf("Текст введен")
    }
    InputText(
        text = data,
        onValueChange = {}
    ) {
        Text(text = "Не должен показаться")
    }
}