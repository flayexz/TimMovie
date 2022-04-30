import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserController } from './controllers/user/user.controller';
import {UserService} from "./services/UserService";
import {AdminAuth} from "./auth/adminAuth";
import {AdminController} from "./controllers/user/adminController";
import {JwtModule} from "@nestjs/jwt";
import { ConfigModule } from '@nestjs/config';

@Module({
  imports: [
      TypeOrmModule.forRoot(),
      ConfigModule.forRoot({
          envFilePath: '.env.'+process.env.NODE_ENV
      }),
      JwtModule.register({})
  ],
  controllers: [UserController,AdminController],
  providers: [UserService,AdminAuth],
})

export class AppModule {}

