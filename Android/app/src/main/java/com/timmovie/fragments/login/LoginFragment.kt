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
fun LoginFragment(login: String, loginChange: (String) -> Unit,
                  password: String, passwordChange: (String) -> Unit,
                  onButtonClick: () -> Unit, buttonEnabled: Boolean) {
    val defaultPadding = 5.dp;
    Scaffold(
        topBar = {
            TopAppBar(modifier = Modifier.fillMaxWidth()) {
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
                        onValueChange = loginChange,
                        modifier = Modifier
                            .padding(bottom = defaultPadding))

                }
                Row(horizontalArrangement = Arrangement.Center, modifier = Modifier.fillMaxWidth()) {
                    Button(
                        onClick = onButtonClick,
                        enabled = buttonEnabled,
                        modifier = Modifier
                    ) {
                        Text(text = "Войти")
                    }

                }
            }
        }
    }
}