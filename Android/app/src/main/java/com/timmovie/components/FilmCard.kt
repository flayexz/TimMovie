package com.timmovie.components

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp

@Composable
fun FilmCard (name: String, image: String, onButtonClick: () -> Unit, length: Int = 10) {
    val context = LocalContext.current
    Column(
        modifier = Modifier
            .clickable {
                onButtonClick()
            }
            .padding(8.dp),
        horizontalAlignment = Alignment.CenterHorizontally,
        verticalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        Box(
            modifier = Modifier
                .size(width = 120.dp, height = 180.dp)
                .background(color = Color.Red, shape = RoundedCornerShape(6.dp))
        )
        Text(text = name.takeIf { it.length <= length } ?: "${name.take(length)}...")
    }}