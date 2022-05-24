import {Body, Controller, Get, Post, Query, UploadedFile, UseInterceptors} from '@nestjs/common';
import {FilmService} from "./FilmService";
import {NewFilmDto} from "../../dto/NewFilmDto";
import {FileInterceptor} from "@nestjs/platform-express";
import {Result} from "../../dto/Result";
import PaginationLoading from "../../dto/PaginationLoading";
import FilmForTableDto from "../../dto/FilmForTableDto";
import NameDto from "../../dto/NameDto";

@Controller('films')
export class FilmController {
    constructor(private readonly filmService: FilmService) {
    }
    
    @Post("add")
    @UseInterceptors(FileInterceptor('img'))
    async addNewFilm(@Body() newFilmInfo: NewFilmDto, @UploadedFile() image: Express.Multer.File): Promise<Result<string>>{
        if (image == null){
            return {
                success: false,
                textError: "Необходимо добавить обложку для фильма",
            }     
        }
        
        return await this.filmService.addNewFilm(newFilmInfo, image);
    }

    @Get("pagination")
    async getFilmByNamePart(@Query() pagination: PaginationLoading): Promise<FilmForTableDto[]> {
        return await this.filmService.getFilmByNamePart(pagination);
    }
    
    @Get("collection")
    async getFilmsTitlesByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.filmService.getFilmsTitlesByNamePart(namePart, skip, take);
    }
}
