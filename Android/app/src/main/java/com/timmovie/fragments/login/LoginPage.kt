package com.timmovie.fragments.login

import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material.Button
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.constraintlayout.compose.ConstraintLayout
import com.timmovie.components.InputText

@Composable
fun LoginPage(login: String, loginChange: (String) -> Unit,
              password: String, passwordChange: (String) -> Unit,
              onButtonClick: () -> Unit, buttonEnabled: Boolean) {
    val defaultPadding = 5.dp;
    ConstraintLayout(modifier = Modifier.width(600.dp)) {
        val (loginRef, passwordRef, buttonRef) = createRefs()
        InputText(
            text = login,
            placeholder = {
                Text(text = "Логин")
            },
            onValueChange = loginChange,
            modifier = Modifier
                .constrainAs(loginRef) {
                    top.linkTo(parent.top)
                    bottom.linkTo(passwordRef.top)
                    start.linkTo(parent.start)
                    end.linkTo(parent.end)
                }
                .padding(bottom = defaultPadding))
        InputText(
            text = password,
            placeholder = {
                Text("Пароль")
            },
            visualTransformation = PasswordVisualTransformation(),
            onValueChange = passwordChange,
            modifier = Modifier.constrainAs(passwordRef) {
                top.linkTo(loginRef.bottom)
                bottom.linkTo(buttonRef.top)
                start.linkTo(parent.start)
                end.linkTo(parent.end)
            }.padding(bottom = defaultPadding)
        )
        Button(
            onClick = onButtonClick,
            enabled = buttonEnabled,
            modifier = Modifier.constrainAs(buttonRef) {
                top.linkTo(passwordRef.bottom)
                bottom.linkTo(parent.bottom)
                start.linkTo(parent.start)
                end.linkTo(parent.end)
            }
        ) {
            Text(text = "Войти")
        }
    }
}

@Composable
@Preview
fun LoginPagePreview() {
    LoginPage("Логин", {},
        "Пароль", {},
        {}, true)
}