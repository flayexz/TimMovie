package com.domain

interface ILoginService {
    suspend fun register(username: String, password: String): Boolean
    suspend fun login(username: String, password: String): Boolean
}