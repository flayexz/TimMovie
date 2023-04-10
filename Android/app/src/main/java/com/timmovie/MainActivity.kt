package com.timmovie

import android.annotation.SuppressLint
import android.os.Bundle
import android.support.v4.media.MediaMetadataCompat.TextKey
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.material.Scaffold
import androidx.compose.material.Surface
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import com.timmovie.fragments.login.LoginPage
import com.timmovie.theme.TimMovieTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            TimMovieTheme {
                
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
}

@SuppressLint("UnusedMaterialScaffoldPaddingParameter")
@Composable
fun LoginPageMain(login: String, onLoginChange: (String) -> Unit, password: String, onPasswordChange: (String) -> Unit, onButtonClick: () -> Unit, buttonEnabled: Boolean) {
    Scaffold(
        topBar = {
            TopAppBar { Text("app bar") }
        }) {
        Row(verticalAlignment = Alignment.CenterVertically, modifier = Modifier.fillMaxHeight()) {
            LoginPage(
                login = login,
                loginChange = onLoginChange,
                password = password,
                passwordChange = onPasswordChange,
                onButtonClick = onButtonClick,
                buttonEnabled = buttonEnabled
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