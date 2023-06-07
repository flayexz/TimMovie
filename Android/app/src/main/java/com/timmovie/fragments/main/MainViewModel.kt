package com.timmovie.fragments.main

import android.os.StrictMode
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.NavController
import com.apollographql.apollo3.ApolloClient
import com.timmovie.GetFilmsQuery
import com.timmovie.MainActivity
import com.timmovie.components.FilmDataImage
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import com.timmovie.theme.Constants
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
import okhttp3.*
import java.io.IOException
import java.util.logging.Logger
import javax.inject.Inject


@HiltViewModel
class MainViewModel @Inject constructor(private val machine: AppStateMachine) : ViewModel() {
    val films = mutableListOf<FilmDataImage>()  
    
    init {
        getFilms()
    }

    fun main() {
        viewModelScope.launch {
            machine.currentState.value = AppState.AllFilms
        }
    }
    
    fun login(){
        viewModelScope.launch {
            machine.currentState.value = AppState.Login
        }
    }

    fun chat(){
        viewModelScope.launch {
            machine.currentState.value = AppState.Chat
        }
    }

    fun goToFilm(controller: NavController, id: String) {
        viewModelScope.launch {
            val link = Constants.Routes.CONCRETEFILM.replace("{id}", id)
            controller.navigate(link)
            val policy = StrictMode.ThreadPolicy.Builder().permitAll().build()
            StrictMode.setThreadPolicy(policy)

            val client = OkHttpClient()

            val request = Request.Builder()
                .url(Constants.Urls.FILMAPI + id)
                .get()
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onResponse(call: Call, response: Response) {
                    if (response.isSuccessful) {
                        val Log = Logger.getLogger(MainActivity::class.java.name)
                        Log.warning("ZAEBUMBA")
                    }
                    response.close()
                }

                override fun onFailure(call: Call, e: IOException) {
                }
            })
        }
    }
    
    private fun getFilms(){
        runBlocking  {
            val apolloClient = ApolloClient.Builder()
                .serverUrl(Constants.Urls.GRAPHQL)
                .build()

            val response = apolloClient.query(GetFilmsQuery()).execute()
            
            val films1: List<GetFilmsQuery.Node> = response.data?.films?.all?.nodes ?: emptyList()

            for (film in films1) {
                val filmData = FilmDataImage(film.id.toString(), film.title, film.image.toString())
                films.add(filmData)
            }
        }
    }
}