package com.timmovie.fragments.login

import android.widget.Toast
import androidx.compose.runtime.*
import androidx.compose.ui.platform.LocalContext
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController

@Composable
fun LoginPage() {
    val nav = rememberNavController()
    val context = LocalContext.current
    NavHost(navController = nav, startDestination = "login") {
        composable("login") {
            var login by remember {
                mutableStateOf("")
            }
            var password by remember {
                mutableStateOf("")
            }
            LoginFragment(
                login = login,
                loginChange = {
                    login = it
                },
                password = password,
                passwordChange = {
                    password = it
                },
                onButtonClick = {
                    Toast.makeText(context, "Пока нет логики. Сасать", Toast.LENGTH_SHORT).show()
                },
                buttonEnabled = login.isNotEmpty() && password.isNotEmpty(),
                onRegisterButtonClick = {
                    nav.navigate("register")
                }
            )
        }
        composable("register") {
            var login by remember {
                mutableStateOf("")
            }
            var password by remember {
                mutableStateOf("")
            }
            var passwordRepeat by remember {
                mutableStateOf("")
            }
            RegisterFragment(
                login = login,
                onLoginChange = {login = it},
                password = password,
                onPasswordChange = {password = it},
                passwordRepeat = passwordRepeat,
                onPasswordRepeatChange = {passwordRepeat = it},
                buttonEnabled = login.isNotEmpty() && password.isNotEmpty() && password == passwordRepeat,
                onButtonClick = {
                    Toast.makeText(context, "Пока нет логики", Toast.LENGTH_SHORT).show()
                },
                onLoginButtonClick = {
                    nav.navigate("login")
                }
            )
        }
    }
}