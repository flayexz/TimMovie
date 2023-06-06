package com.timmovie.fragments.chat

import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.timmovie.fragments.chat.chat_admin.ChatAdminPage
import com.timmovie.fragments.chat.chat_admin.ChatAdminViewModel

const val chatAdminPage = "chat-admin"

@Composable
fun ChatPage(viewModel: ChatViewModel, ) {
    val nav = rememberNavController()
    NavHost(navController = nav, startDestination = chatAdminPage) {
        composable(chatAdminPage) {
            ChatAdminPage(
                ChatAdminViewModel(viewModel.machine)
            )
        }
    }
}