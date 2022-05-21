import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ConfigModule } from '@nestjs/config';
import {UserModule} from "./modules/userModule/user.module";
import {SubscribeModule} from "./modules/subscribesModule/subscribe.module";
import {AuthModule} from "./modules/authModule/auth.module";
import {RoleModule} from "./modules/roleModule/role.module";
import {BannerModule} from "./modules/bannerModule/bannerModule";

@Module({
    imports: [
        TypeOrmModule.forRoot(),
        ConfigModule.forRoot({
            envFilePath: '.env.'+process.env.NODE_ENV
        }),
        AuthModule,
        RoleModule,
        UserModule,
        SubscribeModule,
        BannerModule
    ]
})
export class AppModule {}

