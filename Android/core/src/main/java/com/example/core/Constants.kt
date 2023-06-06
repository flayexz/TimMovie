package com.example.core

object Constants {
    object Routes {
        const val LOGIN = "/login"
        const val CHAT = "/chat"
        const val ALLFILMS = "/allfilms"
        const val CONCRETEFILM = "/films/{id}"
    }
    
    object Urls {
        const val GRAPHQL = "http://10.0.2.2:5011/graphql"
        const val FILESAPI = "http://10.0.2.2:8081/"
        const val FILMAPI = "http://10.0.2.2:5011/statistic/IncreaseFilmTraffic?filmId="
        const val HOST = "10.0.2.2"
        const val PORT = 5011
    }
}

class User {
    companion object {
        var name: String = ""
    }
}