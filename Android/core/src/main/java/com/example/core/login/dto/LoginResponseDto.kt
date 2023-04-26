package com.example.core.login.dto

import kotlinx.serialization.Serializable


@Serializable
data class LoginResponseDto(val token: String) {
}