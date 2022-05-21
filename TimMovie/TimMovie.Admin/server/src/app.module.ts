import { Module } from '@nestjs/common';
import { TypeOrmModule } from '@nestjs/typeorm';
import { ConfigModule } from '@nestjs/config';
import {UserModule} from "./modules/userModule/user.module";
import {SubscribeModule} from "./modules/subscribesModule/subscribe.module";
import {AuthModule} from "./modules/authModule/auth.module";
import {RoleModule} from "./modules/roleModule/role.module";
import {FilmModule} from "./modules/filmModule/film.module";
import {ActorModule} from "./modules/actorModule/actor.module";
import {ProducerModule} from "./modules/producerModule/producer.module";
import {CountryModule} from "./modules/countryModule/countryModule";
import {GenreModule} from "./modules/genreModule/genre.module";


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
        FilmModule,
        ActorModule,
        ProducerModule,
        CountryModule,
        GenreModule,
    ],
})
export class AppModule {}

