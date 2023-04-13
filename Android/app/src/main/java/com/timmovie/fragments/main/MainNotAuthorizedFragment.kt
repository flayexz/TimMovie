package com.timmovie.fragments.main

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.Button
import androidx.compose.material.Scaffold
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.timmovie.components.FilmCard

@Composable
fun MainNotAuthorizedFragment() {
    Scaffold(
        topBar = {
            TopAppBar(modifier = Modifier.fillMaxWidth()) {}
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
                LazyColumn(
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(top = 16.dp, bottom = 16.dp),
                    verticalArrangement = Arrangement.spacedBy(8.dp))
                {
                    items(9) { index ->
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            FilmCard(name = "Не рикролл $index")
                            FilmCard(name = "Не рикролл ${index + 1}")
                            FilmCard(name = "Не рикролл ${index + 2}")
                        }
                    }
                }            
            }
            Box() {
                Button(
                    onClick = {},
                    modifier = Modifier.fillMaxWidth()) 
                {
                    Text(text = "Войти")
                }
            }
            Box() {
                Button(
                    onClick = {},
                    modifier = Modifier.fillMaxWidth()) {
                    Text(text = "Зарегистрироваться")
                }
            }
        }
    }
}

@Preview
@Composable
fun MainNotAuthorizedFragmentPreview() {
    MainNotAuthorizedFragment()
}
