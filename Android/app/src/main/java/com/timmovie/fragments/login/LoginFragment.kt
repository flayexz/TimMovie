package com.timmovie.fragments.login

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.width
import androidx.compose.material.Button
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.ComposeView
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.constraintlayout.compose.ConstraintLayout
import com.timmovie.fragments.login.components.InputText

class LoginFragment : Fragment() {
    companion object {
        fun newInstance() = LoginFragment()
    }

    private lateinit var viewModel: LoginViewModel

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return ComposeView(requireContext()).apply {
            setContent {
                var login by remember {
                    mutableStateOf("")
                }
                var password by remember {
                    mutableStateOf("")
                }
                LoginPage(
                    login = login,
                    loginChange = { login = it },
                    password = password,
                    passwordChange = { password = it },
                    {}, true
                )
            }
        }
    }

    override fun onActivityCreated(savedInstanceState: Bundle?) {
        super.onActivityCreated(savedInstanceState)
        viewModel = ViewModelProvider(this).get(LoginViewModel::class.java)
        // TODO: Use the ViewModel
    }
}

@Composable
fun LoginPage(login: String, loginChange: (String) -> Unit,
              password: String, passwordChange: (String) -> Unit,
              onButtonClick: () -> Unit, buttonEnabled: Boolean) {
    ConstraintLayout(modifier = Modifier.width(600.dp)) {
        val (loginRef, passwordRef, buttonRef) = createRefs()
        InputText(
            text = login,
            placeholder = {
                Text(text = "Логин")
            },
            onValueChange = loginChange,
            modifier = Modifier.constrainAs(loginRef) {
                top.linkTo(parent.top)
                bottom.linkTo(passwordRef.top)
                start.linkTo(parent.start)
                end.linkTo(parent.end)
            })
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
            }
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