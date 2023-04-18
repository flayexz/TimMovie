package com.example.core.login

import com.domain.ILoginService
import kotlinx.coroutines.delay

class NullLoginService: ILoginService {
    override suspend fun register(username: String, password: String): Boolean {
        delay(1000)
        return true
    }

    override suspend fun login(username: String, password: String): Boolean {
        delay(1000)
        return true
    }
}