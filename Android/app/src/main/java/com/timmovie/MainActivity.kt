package com.timmovie

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.material.Surface
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.fragments.login.LoginPage

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            var login by remember {
                mutableStateOf("")
            }
            var password by remember {
                mutableStateOf("")
            }
            var buttonEnabled by remember {
                mutableStateOf(false)
            }
            LoginPageMain(
                login = login,
                onLoginChange = {
                    buttonEnabled = it.isNotEmpty() && password.isNotEmpty()
                    login = it
                },
                password = password,
                onPasswordChange = {
                    buttonEnabled = it.isNotEmpty() && login.isNotEmpty()
                    password = it
                },
                onButtonClick = {  },
                buttonEnabled = buttonEnabled
            )
        }
    }
}

@Composable
fun LoginPageMain(login: String, onLoginChange: (String) -> Unit, password: String, onPasswordChange: (String) -> Unit, onButtonClick: () -> Unit, buttonEnabled: Boolean) {
    Surface(
        modifier = Modifier.fillMaxSize()
    ) {
        Row(
            modifier = Modifier.fillMaxWidth(),
            verticalAlignment = Alignment.CenterVertically
        ) {
            LoginPage(
                login, onLoginChange,
                password, onPasswordChange,
                onButtonClick, buttonEnabled
            )
        }
    }
}

@Preview(showBackground = true)
@Composable
fun DefaultPreview() {
    LoginPageMain(
        login = "Логин",
        onLoginChange = {},
        password = "Пароль",
        onPasswordChange ={} ,
        onButtonClick = { /*TODO*/ },
        buttonEnabled = true
    )
}