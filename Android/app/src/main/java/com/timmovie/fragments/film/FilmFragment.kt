package com.timmovie.fragments.film

import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Button
import androidx.compose.material.Scaffold
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Alignment.Companion.CenterHorizontally
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color.Companion.Red
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp

@Composable
fun FilmFragment(onMainButtonClick: () -> Unit = {}) {
    Scaffold(
        topBar = {
            TopAppBar(modifier = Modifier.fillMaxWidth()) {
                Row(horizontalArrangement = Arrangement.End, modifier = Modifier.fillMaxWidth()) {
                    Text(text = "На главную", fontSize = 16.sp, modifier = Modifier.clickable(onClick = onMainButtonClick))
                }
            }
        },
        modifier = Modifier, 
        bottomBar = {}
    ) {
        it.calculateTopPadding()
        Column(
            modifier = Modifier
                .fillMaxHeight()
                .padding(5.dp)) {
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .weight(1f), 
                contentAlignment = Alignment.Center) 
            {
                Column(
                    horizontalAlignment = CenterHorizontally, 
                    verticalArrangement = Arrangement.spacedBy(8.dp))
                {
                    Box(
                        modifier = Modifier
                            .size(width = 350.dp, height = 250.dp)
                            .background(color = Red, shape = RoundedCornerShape(6.dp)))
                    Text(text = "Не рикролл")
                }
            }
            Box() {
                Button(
                    onClick = {},
                    modifier = Modifier.fillMaxWidth()) 
                {
                    Text(text = "В чат")
                }
            }
            Box() {
                Button(
                    onClick = {},
                    modifier = Modifier.fillMaxWidth()) {
                    Text(text = "К пахану")
                }
            }
        }
    }
}

@Preview
@Composable
fun FilmFragmentPreview() {
    FilmFragment()
}
