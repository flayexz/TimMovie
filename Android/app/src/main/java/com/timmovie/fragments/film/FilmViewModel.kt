package com.timmovie.fragments.film

import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
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
class FilmViewModel @Inject constructor(private val machine: AppStateMachine) : ViewModel() {
    var filmId by mutableStateOf("")
    var filmTitle by mutableStateOf("")
    var url by mutableStateOf("")

    fun main() {
        viewModelScope.launch {
            machine.currentState.value = AppState.AllFilms
        }
    }

    fun chat() {
        viewModelScope.launch {
            machine.currentState.value = AppState.Chat
        }
    }

}