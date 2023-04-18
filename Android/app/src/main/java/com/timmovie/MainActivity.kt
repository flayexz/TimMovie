package com.timmovie

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateListOf
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.core.login.NullLoginService
import com.timmovie.components.ChatRecordItem
import com.timmovie.fragments.chat.ChatPage
import com.timmovie.fragments.chat.ChatViewModel
import com.timmovie.fragments.chat.chat_admin.ChatAdminPage
import com.timmovie.fragments.chat.chat_general.ChatGeneralPage
import com.timmovie.fragments.login.LoginPage
import com.timmovie.fragments.login.LoginViewModel
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import com.timmovie.theme.TimMovieTheme


class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        val machine = AppStateMachine()
        setContent {
            val nav = rememberNavController()

            TimMovieTheme {
                NavHost(navController = nav, startDestination = AppState.Login.name) {
                    composable(AppState.Login.name) {
                        LoginPage(LoginViewModel(NullLoginService(), machine))
                    }
                    composable(AppState.Chat.name) {
                        ChatPage(ChatViewModel(machine = machine))
                    }
                }
            }
            machine.currentState.observe(this) {
                when (it) {
                    AppState.Login -> nav.navigate(AppState.Login.name)
                    AppState.Chat -> nav.navigate(AppState.Chat.name)
                    AppState.AllFilms -> TODO()
                    AppState.ConcreteFilm -> TODO()
                }
            }
        }
        machine.currentState.value = AppState.Login
    }
}


//@Preview(showBackground = true)
//@Composable
//fun DefaultPreview() {
//    TimMovieTheme {
//        LoginPage()
//    }
//}