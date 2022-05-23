import { Module } from '@nestjs/common';
import { FilmController } from './film.controller';
import {FilmService} from "./FilmService";
import {FileService} from "../FileService";

@Module({
  controllers: [FilmController],
  providers: [FilmService, FileService]
})
export class FilmModule {}
