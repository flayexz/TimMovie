package com.timmovie.fragments.login

import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController

@Composable
fun LoginPage(viewModel: LoginViewModel) {
    val nav = rememberNavController()
    NavHost(navController = nav, startDestination = "login") {
        composable("login") {
            LoginFragment(
                login = viewModel.login,
                loginChange = {
                    viewModel.login = it
                },
                password = viewModel.password,
                passwordChange = {
                    viewModel.password = it
                },
                onButtonClick = {
//                    Toast.makeText(context, "Пока нет логики. Сасать", Toast.LENGTH_SHORT).show()
                    viewModel.login()
                },
                buttonEnabled = viewModel.login.isNotEmpty()
            )
        }
    }
}