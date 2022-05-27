import {Controller, Get, Query} from '@nestjs/common';
import NameDto from "../../dto/NameDto";
import {GenreService} from "./ProducerService";
import {Admin} from "../authModule/adminAuth";

@Admin()
@Controller('genres')
export class GenreController {
    constructor(private readonly genreService: GenreService) {
    }

    @Get("collection")
    async getProducersByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.genreService.getGenresByNamePart(namePart, skip, take);
    }
}
