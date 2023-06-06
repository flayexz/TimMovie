package com.timmovie.infrastructure

import com.domain.login.ILoginService
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