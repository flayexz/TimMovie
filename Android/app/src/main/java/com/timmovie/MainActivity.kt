package com.timmovie

//import com.timmovie.infrastructure.AppModule
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import com.example.core.Constants
import com.timmovie.fragments.chat.ChatPage
import com.timmovie.fragments.chat.ChatViewModel
import com.timmovie.fragments.film.FilmPage
import com.timmovie.fragments.film.FilmViewModel
import com.timmovie.fragments.login.LoginPage
import com.timmovie.fragments.login.LoginViewModel
import com.timmovie.fragments.main.MainPage
import com.timmovie.fragments.main.MainViewModel
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
                NavHost(navController = nav, startDestination = Constants.Routes.LOGIN) {
                    composable(Constants.Routes.LOGIN) {
                        val viewModel = hiltViewModel<LoginViewModel>()
                        LoginPage(viewModel)
                    }
                    composable(Constants.Routes.CHAT) {
                        val viewModel = hiltViewModel<ChatViewModel>()
                        ChatPage(viewModel)
                    }
                    composable(Constants.Routes.ALLFILMS) {
                        val viewModel = hiltViewModel<MainViewModel>()
                        viewModel.getFilms()
                        MainPage(viewModel, nav)
                    }
                    composable(Constants.Routes.CONCRETEFILM, arguments = listOf(navArgument("id") {
                        type = NavType.StringType
                    })) {
                        val viewModel = hiltViewModel<FilmViewModel>()
                        viewModel.filmId
                        val filmId = it.arguments?.getString("id")
                        filmId?.let {
                            viewModel.filmId = filmId
                        }
                        FilmPage(viewModel)
                    }
                }
            }
            machine.currentState.observe(this) {
                when (it) {
                    AppState.Login -> nav.navigate(Constants.Routes.LOGIN)
                    AppState.Chat -> nav.navigate(Constants.Routes.CHAT)
                    AppState.AllFilms -> nav.navigate(Constants.Routes.ALLFILMS)
                    AppState.ConcreteFilm -> nav.navigate(Constants.Routes.CONCRETEFILM)
                }
            }
            machine.currentState.value = AppState.Login
        }
    }
}
