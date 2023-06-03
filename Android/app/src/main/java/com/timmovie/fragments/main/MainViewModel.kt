package com.timmovie.fragments.main

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import androidx.navigation.NavController
import com.example.core.Constants
import com.timmovie.components.FilmDataImage
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import javax.inject.Inject

@HiltViewModel
class MainViewModel @Inject constructor(private val machine: AppStateMachine) : ViewModel() {
    val films = mutableListOf<FilmDataImage>()

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

    fun goToFilm(controller: NavController, id: String) {
        viewModelScope.launch {
            val link = Constants.Routes.CONCRETEFILM.replace("{id}", id)
            controller.navigate(link)
        }
    }
    
    fun getFilms(){
        viewModelScope.launch {
            films.add(FilmDataImage("11", "11", "png"))
            films.add(FilmDataImage("22", "22", "png"))
            films.add(FilmDataImage("33", "33", "png"))
        }
    }
}