package com.example.core.login

import com.domain.login.ILoginService
import com.example.core.login.dto.RegisterRequestDto
import com.example.core.login.dto.RegisterResponseDto
import io.ktor.client.HttpClient
import io.ktor.client.call.body
import io.ktor.client.request.post
import io.ktor.client.request.setBody
import io.ktor.http.ContentType
import io.ktor.http.HttpStatusCode
import io.ktor.http.contentType
import io.ktor.http.isSuccess

class OkHttpLoginService(private val client: HttpClient): ILoginService {
    override suspend fun register(username: String, password: String): Boolean {
        val response = client.post(registerUrl) {
            setBody(RegisterRequestDto(
                username = username,
                password = password
            ))
            contentType(ContentType.Application.Json)
        }
        return response.status.isSuccess()
    }

    override suspend fun login(username: String, password: String): Boolean {
        val response = client.post(loginUrl) {
            setBody(RegisterRequestDto(
                username = username,
                password = password
            ))
            contentType(ContentType.Application.Json)
        }
        return response.status.isSuccess()
    }


    companion object {
        private const val baseUrl = "http://localhost:8080"
        private const val registerUrl = "$baseUrl/api/register"

        private const val loginUrl = "$baseUrl/api/login"
    }
}