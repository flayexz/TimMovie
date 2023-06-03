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

@Composable
fun MainAuthorizedFragment(films: List<FilmDataImage>, onButtonClick: () -> Unit, onButton2Click: (controller: NavController, id: String) -> Unit, controller: NavController) {
    val context = LocalContext.current
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
                    items(films.size / 3 - 1) { index ->
                        Row(
                            modifier = Modifier.fillMaxWidth(),
                            horizontalArrangement = Arrangement.SpaceBetween
                        ) {
                            FilmCard(name = films[index].filmTitle, films[index].image, onButtonClick = { onButton2Click(controller, films[index].filmId) } )
                            FilmCard(name = films[index + 1].filmTitle, films[index + 1].image, onButtonClick = { onButton2Click(controller, films[index + 1].filmId) })
                            FilmCard(name = films[index + 2].filmTitle, films[index + 2].image, onButtonClick = { onButton2Click(controller, films[index + 2].filmId) })
                        }
                    }
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