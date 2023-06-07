package com.timmovie.fragments.main

import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.material.Button
import androidx.compose.material.Scaffold
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavController
import com.timmovie.components.FilmCard
import com.timmovie.components.FilmDataImage
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.withContext

@Composable
fun MainAuthorizedFragment(films: List<FilmDataImage>, onButtonClick: () -> Unit, onButton2Click: (controller: NavController, id: String) -> Unit, controller: NavController, onButton3Click: () -> Unit,) {
    Scaffold(
        topBar = {
            TopAppBar(modifier = Modifier.fillMaxWidth()) {
                Row(horizontalArrangement = Arrangement.End, modifier = Modifier.fillMaxWidth()) {
                    Text(text = "Выход", fontSize = 16.sp, modifier = Modifier.clickable(onClick = onButtonClick))
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
                LazyColumn(
                    modifier = Modifier
                        .fillMaxSize()
                        .padding(top = 16.dp, bottom = 16.dp),
                    verticalArrangement = Arrangement.spacedBy(8.dp))
                {
                    items(films.size / 3) { index ->
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            val firstIndex = index * 3
                            val secondIndex = firstIndex + 1
                            val thirdIndex = firstIndex + 2

                            if (firstIndex < films.size) {
                                FilmCard(name = films[firstIndex].filmTitle, films[firstIndex].image, onButtonClick = { onButton2Click(controller, films[firstIndex].filmId) })
                            }
                            if (secondIndex < films.size) {
                                FilmCard(name = films[secondIndex].filmTitle, films[secondIndex].image, onButtonClick = { onButton2Click(controller, films[secondIndex].filmId) })
                            }
                            if (thirdIndex < films.size) {
                                FilmCard(name = films[thirdIndex].filmTitle, films[thirdIndex].image, onButtonClick = { onButton2Click(controller, films[thirdIndex].filmId) })
                            }
                        }
                    }
                }            
            }
            Box() {
                Button(
                    onClick = onButton3Click,
                    modifier = Modifier.fillMaxWidth()) {
                    Text(text = "К пахану")
                }
            }
        }
    }
}