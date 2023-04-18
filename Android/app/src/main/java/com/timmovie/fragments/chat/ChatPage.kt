package com.timmovie.fragments.chat

import androidx.compose.runtime.Composable
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import com.example.core.chat.NullAdminChatService
import com.example.core.chat.NullGeneralChatService
import com.timmovie.fragments.chat.chat_admin.ChatAdminPage
import com.timmovie.fragments.chat.chat_admin.ChatAdminViewModel
import com.timmovie.fragments.chat.chat_general.ChatGeneralPage
import com.timmovie.fragments.chat.chat_general.ChatGeneralViewModel

const val chatAdminPage = "chat-admin"
const val chatGeneralPage = "chat-general"

@Composable
fun ChatPage(viewModel: ChatViewModel) {
    val nav = rememberNavController()
    NavHost(navController = nav, startDestination = chatGeneralPage) {
        composable(chatGeneralPage) {
            ChatGeneralPage(
                ChatGeneralViewModel(viewModel.machine, NullGeneralChatService.Instance),
                onToGeneralPageClick = {
                    nav.navigate(chatAdminPage)
                }
            )
        }
        composable(chatAdminPage) {
            ChatAdminPage(
                ChatAdminViewModel(viewModel.machine, NullAdminChatService.Instance),
                onToGeneralChatClick = {
                    nav.navigate(chatGeneralPage)
                }
            )
        }
    }
}