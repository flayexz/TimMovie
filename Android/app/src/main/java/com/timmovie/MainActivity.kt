package com.timmovie

import android.annotation.SuppressLint
import android.os.Bundle
import android.support.v4.media.MediaMetadataCompat.TextKey
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.*
import androidx.compose.material.Scaffold
import androidx.compose.material.Surface
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
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