import {Body, Controller, Post, UploadedFile, UseInterceptors} from '@nestjs/common';
import {FilmService} from "./FilmService";
import {NewFilm} from "../../dto/NewFilm";
import {FileInterceptor} from "@nestjs/platform-express";
import {Result} from "../../dto/Result";


@Controller('films')
export class FilmController {
    constructor(private readonly filmService: FilmService) {
    }
    
    @Post("add")
    @UseInterceptors(FileInterceptor('img'))
    async addNewFilm(@Body() newFilm: NewFilm, @UploadedFile() image: Express.Multer.File): Promise<Result<string>>{
        if (image == null){
            return {
                success: false,
                textError: "Необходимо добавить обложку для фильма",
            }     
        }
        
        return await this.filmService.addNewFilm(newFilm, image);
    }
}
