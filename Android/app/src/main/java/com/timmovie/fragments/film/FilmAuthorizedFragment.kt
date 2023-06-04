package com.timmovie.fragments.film

import android.annotation.SuppressLint
import android.util.Log
import android.webkit.WebChromeClient
import android.webkit.WebSettings
import android.webkit.WebView
import android.webkit.WebViewClient
import androidx.compose.foundation.background
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.Button
import androidx.compose.material.Scaffold
import androidx.compose.material.Text
import androidx.compose.material.TopAppBar
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Alignment.Companion.CenterHorizontally
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color.Companion.Red
import androidx.compose.ui.graphics.Color.Companion.White
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.compose.ui.viewinterop.AndroidView
import com.pierfrancescosoffritti.androidyoutubeplayer.core.player.YouTubePlayer
import com.pierfrancescosoffritti.androidyoutubeplayer.core.player.listeners.AbstractYouTubePlayerListener
import com.pierfrancescosoffritti.androidyoutubeplayer.core.player.views.YouTubePlayerView
import com.timmovie.MainActivity
import java.util.logging.Logger

@Composable
fun FilmAuthorizedFragment(
    filmId: String,
    filmTitle: String,
    url: String,
    onButtonClick: () -> Unit,
    onButton2Click: () -> Unit
) {
    val context = LocalContext.current

    Scaffold(
        topBar = {
            TopAppBar(modifier = Modifier.fillMaxWidth()) {
                Row(horizontalArrangement = Arrangement.End, modifier = Modifier.fillMaxWidth()) {
                    Text(
                        text = "На главную",
                        fontSize = 16.sp,
                        modifier = Modifier.clickable(onClick = onButtonClick)
                    )
                }
            }
        },
        modifier = Modifier,
        bottomBar = {}
    ) {
        it.calculateTopPadding()
        Column(
            modifier = Modifier
                .fillMaxHeight()
                .padding(5.dp)
        ) {
            Box(
                modifier = Modifier
                    .fillMaxSize()
                    .weight(1f),
                contentAlignment = Alignment.Center
            )
            {
                Column(
                    horizontalAlignment = CenterHorizontally,
                    verticalArrangement = Arrangement.spacedBy(8.dp)
                )
                {
                    Box(
                        modifier = Modifier
                            .size(width = 350.dp, height = 250.dp)
                            .background(color = White, shape = RoundedCornerShape(6.dp))
                    ) {
                        AndroidView(factory = {
                            val Log = Logger.getLogger(MainActivity::class.java.name)
                            Log.warning(url)
                            var view = YouTubePlayerView(it)
                            val fragment = view.addYouTubePlayerListener(
                                object : AbstractYouTubePlayerListener() {
                                    override fun onReady(youTubePlayer: YouTubePlayer) {
                                        super.onReady(youTubePlayer)
                                        youTubePlayer.loadVideo(url, 0f)
                                    }
                                }
                            )
                            view
                        })
                    }
                    Text(text = filmTitle)
                }
            }
            Box() {
                Button(
                    onClick = onButton2Click,
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Text(text = "К пахану")
                }
            }
        }
    }
}
