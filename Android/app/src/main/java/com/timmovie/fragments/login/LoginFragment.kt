package com.timmovie.fragments.login

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import android.renderscript.ScriptGroup.Input
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.compose.foundation.layout.Column
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Modifier
import androidx.compose.runtime.getValue
import androidx.compose.runtime.setValue
import androidx.compose.ui.platform.ComposeView
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.R
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
                var passwordRepeat by remember {
                    mutableStateOf("")
                }
                LoginPage(
                    login = login,
                    loginChange = { login = it },
                    password = password,
                    passwordChange = { password = it },
                    passwordRepeat = passwordRepeat,
                    passwordRepeatChange = { passwordRepeat = it }
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
              passwordRepeat: String, passwordRepeatChange: (String) -> Unit) {
    Column {
        InputText(text = login, onValueChange = loginChange)
        InputText(text = password, onValueChange = passwordChange)
        InputText(text = passwordRepeat, onValueChange = passwordRepeatChange)
    }
}

@Composable
@Preview
fun LoginPagePreview() {
    LoginPage("Логин", {}, "Пароль", {}, "Пароль", {})
}