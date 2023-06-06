package com.timmovie.fragments.film

import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.compose.runtime.livedata.observeAsState
import com.example.core.chat.NullAdminChatService
import com.timmovie.fragments.chat.chat_admin.ChatAdminPage
import com.timmovie.fragments.chat.chat_admin.ChatAdminViewModel
import com.timmovie.fragments.login.LoginFragment

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