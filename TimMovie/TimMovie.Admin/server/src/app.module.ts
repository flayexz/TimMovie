import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { UserController } from './controllers/user/user.controller';
import {UserService} from "./services/UserService";
import {RoleService} from "./services/RoleService";
import {SubscribeService} from "./services/SubscribeService";
import { SubscribeController } from "./controllers/subscribe/subscribeController"

@Module({
  imports: [TypeOrmModule.forRoot()],
  controllers: [UserController, SubscribeController],
  providers: [UserService, RoleService, SubscribeService],
})

export class AppModule {}

