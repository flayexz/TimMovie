import {Injectable} from "@nestjs/common";
import {NewFilm} from "../../dto/NewFilm";
import {Result} from "../../dto/Result";
import {FileService} from "../FileService";
import {getRepository, Raw} from "typeorm";
import {Actor} from "../../../entities/Actor";
import {Film} from "../../../entities/Film";
import {Producer} from "../../../entities/Producer";
import {Genre} from "../../../entities/Genre";
import {Country} from "../../../entities/Country";
import {Guid} from "guid-typescript";


@Injectable()
export class FilmService{
    constructor(private readonly fileService: FileService) {
    }
    
    async addNewFilm(newFilm: NewFilm, image: Express.Multer.File): Promise<Result<string>>{
        let resultSaveImage = await this.fileService.saveFilmImage(image);
        
        if (!resultSaveImage.success){
            return {
                success: false,
                textError: "Во время сохранения  произошла ошибка",
            }
        }
        console.log(Guid.create().toString());
        let film = getRepository(Film).create({
            id: Guid.create().toString(),
            title: newFilm.title,
            description: newFilm.description,
            image: resultSaveImage.result,
            filmLink: newFilm.filmLink,
            isFree: newFilm.isFree,
            year: newFilm.year
        })

        if (newFilm.actorNames != null){
            let actors = await getRepository(Actor)
                .find({
                    where: newFilm.actorNames?.map(value => {
                        let nameAndSurname = value.split(" ");

                        return {
                            name: nameAndSurname[0],
                            surname: nameAndSurname[1] ?? ""
                        }
                    })
                });
            console.log(actors);
            
            film.actors = actors;
        }

        if (newFilm.producerNames != null){
            let producers = await getRepository(Producer)
                .find({
                    where: newFilm.producerNames?.map(value => {
                        let nameAndSurname = value.split(" ");

                        return {
                            name: nameAndSurname[0],
                            surname: nameAndSurname[1] ?? ""
                        }
                    })
                });
            console.log(producers);

            film.producers = producers;
        }

        if (newFilm.genreNames != null){
            let genres = await getRepository(Genre)
                .find({
                    where: newFilm.genreNames?.map(value => {
                        return {
                           name: value
                        }
                    })
                });
            console.log(genres);

            film.genres = genres;
        }
        
        let country = await getRepository(Country)
            .findOne({
                where:{
                    name: newFilm.countryName
                }
            });
        console.log(country);
        film.country = country;

        console.log(film);
        
        await getRepository(Film)
            .save(film);
        
        return {
            success: true,
        };
    }
}