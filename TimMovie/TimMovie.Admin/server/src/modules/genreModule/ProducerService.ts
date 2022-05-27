import {Injectable} from "@nestjs/common";
import NameDto from "../../dto/NameDto";
import {getRepository} from "typeorm";
import {includeNamePart} from "../../common/queryFunction";
import {Producer} from "../../../entities/Producer";
import {Genre} from "../../../entities/Genre";

@Injectable()
export class GenreService{
    async getGenresByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]>{
        if (namePart == null){
            return [];
        }

        let genres = await getRepository(Genre)
            .find({
                where: [
                    {name: includeNamePart(namePart)}
                ],
                take,
                skip,
                order: {
                    name: 'ASC',
                }
            });

        let genresDto = genres.map(genre => {
            return {
                id: genre.id,
                name: genre.name
            }
        });
        return genresDto;
    }

    async getGenresByFullName(genreNames: string[]): Promise<Genre[]>{
        return await getRepository(Genre)
            .find({
                where: genreNames.map(value => {
                    return {
                        name: value
                    }
                })
            });
    }
}