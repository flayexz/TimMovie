package com.timmovie.theme

import io.grpc.ManagedChannel
import io.grpc.ManagedChannelBuilder

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
        const val PORT = 5002
    }
}

class User {
    companion object {
        var name: String = ""
    }
}

val MyChannel: ManagedChannel = ManagedChannelBuilder
//    .forAddress("bcdf-178-205-29-128.ngrok-free.app", 8080)
                .forAddress(Constants.Urls.HOST, Constants.Urls.PORT)
    .usePlaintext()
    .build()

object MyGrpcService {
    val chatStub: ChatGrpc.ChatStub = ChatGrpc.newStub(MyChannel)
}

var isBuilded = false;