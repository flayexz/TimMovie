package com.timmovie

//import com.timmovie.infrastructure.AppModule
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.timmovie.fragments.chat.ChatPage
import com.timmovie.fragments.chat.ChatViewModel
import com.timmovie.fragments.login.LoginPage
import com.timmovie.fragments.login.LoginViewModel
import com.timmovie.infrastructure.AppModule
import com.timmovie.infrastructure.AppState
import com.timmovie.theme.TimMovieTheme
import dagger.hilt.android.AndroidEntryPoint


@AndroidEntryPoint
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            val machine = AppModule.stateMachine
            val nav = rememberNavController()
            TimMovieTheme {
                NavHost(navController = nav, startDestination = AppState.Login.name) {
                    composable(AppState.Login.name) {
                        val viewModel = hiltViewModel<LoginViewModel>()
                        LoginPage(viewModel)
                    }
                    composable(AppState.Chat.name) {
                        val viewModel = hiltViewModel<ChatViewModel>()
                        ChatPage(viewModel)
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
            machine.currentState.value = AppState.Login
        }
    }
}
