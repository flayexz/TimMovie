package com.timmovie.infrastructure

import androidx.lifecycle.MutableLiveData
import javax.inject.Inject
import javax.inject.Singleton


@Singleton
class AppStateMachine @Inject constructor() {
    val currentState = MutableLiveData<AppState>()
}