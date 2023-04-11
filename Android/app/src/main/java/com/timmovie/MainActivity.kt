package com.timmovie

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.tooling.preview.Preview
import com.timmovie.fragments.chat_admin.ChatAdminPage
import com.timmovie.fragments.chat_general.ChatGeneralPage
import com.timmovie.fragments.login.LoginPage
import com.timmovie.theme.TimMovieTheme


class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            TimMovieTheme {
                LoginPage()
            }
        }
    }
}


@Preview(showBackground = true)
@Composable
fun DefaultPreview() {
    TimMovieTheme {
        LoginPage()
    }
}