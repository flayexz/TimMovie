package com.timmovie.infrastructure

import com.domain.chat.IAdminChatService
import com.domain.chat.IGeneralChatService
import com.domain.login.ILoginService
import com.example.core.chat.NullAdminChatService
import com.example.core.chat.NullGeneralChatService
import com.example.core.login.NullLoginService

import dagger.Module
import dagger.Provides
import dagger.hilt.InstallIn
import dagger.hilt.components.SingletonComponent
import javax.inject.Singleton

@InstallIn(SingletonComponent::class)
@Module
object AppModule {
    @Singleton
    @Provides
    fun getAdminChatService(): IAdminChatService {
        return NullAdminChatService()
    }

    @Singleton
    @Provides
    fun getGeneralChatService(): IGeneralChatService {
        return NullGeneralChatService()
    }

    @Singleton
    @Provides
    fun getLoginService(): ILoginService {
        return NullLoginService()
    }

    @Singleton
    @Provides
    fun getAppStateMachine(): AppStateMachine {
        return stateMachine
    }

    val stateMachine = AppStateMachine()
}