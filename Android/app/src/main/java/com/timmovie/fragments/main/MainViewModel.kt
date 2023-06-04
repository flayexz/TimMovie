package com.timmovie.fragments.main

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.NavController
import com.apollographql.apollo3.ApolloClient
import com.example.core.Constants
import com.timmovie.GetFilmsQuery
import com.timmovie.components.FilmDataImage
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
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