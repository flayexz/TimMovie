package com.timmovie.fragments.login

import android.widget.Toast
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.material.Button
import androidx.compose.material.Scaffold
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.timmovie.components.InputText

@Composable
fun RegisterFragment(login: String, onLoginChange: (String) -> Unit,
                     password: String, onPasswordChange: (String) -> Unit,
                     passwordRepeat: String, onPasswordRepeatChange: (String) -> Unit,
                     buttonEnabled: Boolean, onButtonClick: () -> Unit,
                     onLoginButtonClick: () -> Unit = {}) {
    val defaultPadding = 5.dp;
    Scaffold(
        topBar = {
            TopAppBar(modifier = Modifier.fillMaxWidth()) {
                Row(horizontalArrangement = Arrangement.End, modifier = Modifier.fillMaxWidth()) {
                    Text(text = "Вход", fontSize = 16.sp, modifier = Modifier.clickable(onClick = onLoginButtonClick))
                }
            }
        },
        modifier = Modifier
    ) {
        it.calculateTopPadding()
        it.calculateBottomPadding()
        Box(modifier = Modifier
        ) {
            Column(
                modifier = Modifier
                    .fillMaxWidth()
                    .fillMaxHeight(),
                verticalArrangement = Arrangement.Center,
            ) {
                Row(horizontalArrangement = Arrangement.Center,
                    modifier = Modifier.fillMaxWidth()
                ) {
                    InputText(
                        text = login,
                        placeholder = {
                            Text(text = "Логин")
                        },
                        onValueChange = onLoginChange,
                        modifier = Modifier
                            .padding(bottom = defaultPadding))

                }
                Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {
                    InputText(
                        text = password,
                        placeholder = {
                            Text("Пароль")
                        },
                        visualTransformation = PasswordVisualTransformation(),
                        onValueChange = onPasswordChange,
                        modifier = Modifier
                            .padding(bottom = defaultPadding)
                    )
                }
                Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {
                    InputText(
                        text = passwordRepeat,
                        placeholder = {
                            Text("Повторите пароль")
                        },
                        visualTransformation = PasswordVisualTransformation(),
                        onValueChange = onPasswordRepeatChange,
                        modifier = Modifier
                            .padding(bottom = defaultPadding)
                    )
                }
                Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {
                    Button(
                        onClick = onButtonClick,
                        enabled = buttonEnabled,
                        modifier = Modifier
                    ) {
                        Text(text = "Зарегистрироваться")
                    }

                }
            }
        }
    }
}

@Preview
@Composable
fun RegisterFragmentPreview() {
    RegisterFragment(
        login = "Логин",
        onLoginChange = {},
        password = "Пароль",
        onPasswordChange = {},
        passwordRepeat = "Пароль",
        onPasswordRepeatChange = {},
        buttonEnabled = true,
        onButtonClick = {}
    ) {

    }
}
