package com.timmovie.fragments.film

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.compose.runtime.livedata.observeAsState

@Composable
fun FilmPage(viewModel: FilmViewModel) {
    val nav = rememberNavController()
    val statistics = viewModel.statistics.observeAsState()
    LaunchedEffect(Unit) {
        viewModel.subscribeToStatistics()
    }
    NavHost(navController = nav, startDestination = "film") {
        composable("film") {
            FilmAuthorizedFragment(
                filmId = viewModel.filmId,
                filmTitle = viewModel.filmTitle,
                url = viewModel.url,
                onButtonClick = {
                    viewModel.main()
                },
                onButton2Click = {
                    viewModel.chat()
                },
                statistics
            )
        }
    }
}