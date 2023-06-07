package com.timmovie.fragments.login

import android.content.SharedPreferences
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.domain.login.ILoginService
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import com.timmovie.theme.User
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import javax.inject.Inject


@HiltViewModel
class LoginViewModel @Inject constructor(private val loginService: ILoginService,
                                         private val machine: AppStateMachine) : ViewModel() {
//                                         private val sharedPreferences: SharedPreferences) : ViewModel() {
    var login by mutableStateOf("")
    var password by mutableStateOf("")
    fun login() {
        viewModelScope.launch {
            val success = loginService.login(login, password)
            if (success) {
                User.name = login
                machine.currentState.value = AppState.AllFilms
            }
        }
    }
}