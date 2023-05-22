package com.example.core.login.dto

import kotlinx.serialization.Serializable


@Serializable
data class RegisterResponseDto(val token: String) {
}