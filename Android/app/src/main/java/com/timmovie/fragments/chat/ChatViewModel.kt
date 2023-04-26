package com.timmovie.fragments.chat

import androidx.lifecycle.ViewModel
import com.timmovie.infrastructure.AppStateMachine
import dagger.hilt.android.lifecycle.HiltViewModel
import javax.inject.Inject

@HiltViewModel
class ChatViewModel @Inject constructor(val machine: AppStateMachine): ViewModel() {
}