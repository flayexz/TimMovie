import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserController } from './controllers/user/user.controller';
import {UserService} from "./services/UserService";

@Module({
  imports: [TypeOrmModule.forRoot()],
  controllers: [UserController],
  providers: [UserService],
})

export class AppModule {}

