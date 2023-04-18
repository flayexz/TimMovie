package com.timmovie.fragments.login

import android.widget.Toast
import androidx.compose.runtime.*
import androidx.compose.ui.platform.LocalContext
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
                buttonEnabled = viewModel.password.isNotEmpty() && viewModel.login.isNotEmpty(),
                onRegisterButtonClick = {
                    nav.navigate("register")
                }
            )
        }
        composable("register") {
            RegisterFragment(
                login = viewModel.login,
                onLoginChange = {viewModel.login = it},
                password = viewModel.password,
                onPasswordChange = {viewModel.password = it},
                passwordRepeat = viewModel.passwordRepeat,
                onPasswordRepeatChange = {viewModel.passwordRepeat = it},
                buttonEnabled = viewModel.login.isNotEmpty() && viewModel.password.isNotEmpty() && viewModel.password == viewModel.passwordRepeat,
                onButtonClick = {
//                    Toast.makeText(context, "Пока нет логики", Toast.LENGTH_SHORT).show()
                    viewModel.register()
                },
                onLoginButtonClick = {
                    nav.navigate("login")
                }
            )
        }
    }
}