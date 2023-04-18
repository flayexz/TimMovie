package com.timmovie.infrastructure

import androidx.lifecycle.MutableLiveData

class AppStateMachine {
    val currentState = MutableLiveData<AppState>()
}