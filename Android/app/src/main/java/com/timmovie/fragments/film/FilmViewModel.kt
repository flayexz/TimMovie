package com.timmovie.fragments.film

import android.util.Log
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.setValue
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.apollographql.apollo3.ApolloClient
import com.example.core.Constants
import com.timmovie.GetFilmsQuery
import com.timmovie.components.FilmDataImage
import com.timmovie.infrastructure.AppState
import com.timmovie.infrastructure.AppStateMachine
import com.timmovie.infrastructure.RabbitmqStatisticsManagerProvider
import com.timmovie.infrastructure.StatisticsManager
import dagger.hilt.android.lifecycle.HiltViewModel
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
import javax.inject.Inject

@HiltViewModel
class FilmViewModel @Inject constructor(private val machine: AppStateMachine) : ViewModel() {
    var filmId by mutableStateOf("")
    var filmTitle by mutableStateOf("")
    var url by mutableStateOf("")
    val statistics = MutableLiveData<Map<String, Int>>(HashMap())

    private val statisticsManager: StatisticsManager by lazy {
        RabbitmqStatisticsManagerProvider.provideStatisticsManager()
    }

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

    suspend fun subscribeToStatistics() {
        viewModelScope.launch {
            val f = statisticsManager.getFilmStatistics()
            f.collect {
                Log.i("ViewModel", "Получил новый словарь")
                it.forEach {
                    Log.d("Elem", "${it.key}: ${it.value}")
                }
                statistics.value = it
            }
        }
    }
}