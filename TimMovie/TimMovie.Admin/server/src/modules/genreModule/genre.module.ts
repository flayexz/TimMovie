import { Module } from '@nestjs/common';
import { GenreController } from './genre.controller';
import {GenreService} from "./ProducerService";

@Module({
  controllers: [GenreController],
  providers: [GenreService]
})
export class GenreModule {}
