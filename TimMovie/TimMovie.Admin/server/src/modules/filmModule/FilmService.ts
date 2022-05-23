import {Injectable} from "@nestjs/common";
import {NewFilmDto} from "../../dto/NewFilmDto";
import {Result} from "../../dto/Result";
import {FileService} from "../FileService";
import {getRepository} from "typeorm";
import {Film} from "../../../entities/Film";
import {Guid} from "guid-typescript";
import {plainToInstance} from "class-transformer";
import {ActorService} from "../actorModule/ActorService";
import {ProducerService} from "../producerModule/ProducerService";
import {GenreService} from "../genreModule/ProducerService";
import {CountryService} from "../countryModule/country.service";


@Injectable()
export class FilmService{
    constructor(private readonly fileService: FileService,
                private readonly actorService: ActorService,
                private readonly producerService: ProducerService,
                private readonly genreService: GenreService,
                private readonly countryService: CountryService) {
    }
    
    async addNewFilm(newFilm: NewFilmDto, image: Express.Multer.File): Promise<Result<string>>{
        let resultSaveImage = await this.fileService.saveFilmImage(image);
        
        if (!resultSaveImage.success){
            return {
                success: false,
                textError: "Во время сохранения  произошла ошибка",
            }
        }
        
        let filmRepo = getRepository(Film);
        let film = filmRepo.create(plainToInstance(Film, newFilm));
        
        film.id = Guid.create().toString();
        film.image = resultSaveImage.result;

        if (newFilm.actorNames != null){
            
            let actors = await this.actorService.getActorsByFullName(newFilm.actorNames);
            film.actors = actors;
        }

        if (newFilm.producerNames != null){
            let producers = await this.producerService.getProducersByFullName(newFilm.producerNames);
            film.producers = producers;
        }

        if (newFilm.genreNames != null){
            let genres = await this.genreService.getGenresByFullName(newFilm.genreNames);
            film.genres = genres;
        }
        
        let country = await this.countryService.getCountryFullName(newFilm.countryName);
        film.country = country;
        
        await filmRepo.save(film);
        
        return {
            success: true,
        };
    }
}