package com.example.core.login.dto

import kotlinx.serialization.Serializable

@Serializable
data class RegisterRequestDto(val username: String, val password: String)