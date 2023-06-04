package com.timmovie.fragments.main

import androidx.compose.runtime.Composable
import androidx.navigation.NavController
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.core.chat.NullAdminChatService
import com.timmovie.fragments.chat.chat_admin.ChatAdminPage
import com.timmovie.fragments.chat.chat_admin.ChatAdminViewModel
import com.timmovie.fragments.login.LoginFragment

@Composable
fun MainPage(viewModel: MainViewModel, controller: NavController) {
    val nav = rememberNavController()
    NavHost(navController = nav, startDestination = "film") {
        composable("film") {
            MainAuthorizedFragment(
                films = viewModel.films,
                onButtonClick = {
                    viewModel.login()
                },
                onButton2Click = { id, controller ->
                    viewModel.goToFilm(id, controller)
                },
                controller = controller,
                onButton3Click = {
                    viewModel.chat()
                },
            )
        }
    }
}