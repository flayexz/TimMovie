import {Injectable} from "@nestjs/common";
import {getRepository} from "typeorm";
import {Actor} from "../../../entities/Actor";
import {includeNamePart} from "../../common/queryFunction";
import NameDto from "../../dto/NameDto";

@Injectable()
export class ActorService{
    async getActorsByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]>{
        if (namePart == null){
            return [];
        }
        
        let actors = await getRepository(Actor)
            .find({
                where: [
                    {name: includeNamePart(namePart)},
                    {surname: includeNamePart(namePart)}
                ],
                take,
                skip,
                order: {
                    name: 'ASC',
                    surname: "ASC"
                }
            });
        
        let actorsDto = actors.map(actor => {
            return {
                id: actor.id,
                name: `${actor.name} ${actor.surname}`
            }
        });
        return actorsDto;
    }
    
    async getActorsByFullName(fullName: string[]): Promise<Actor[]>{
        return await getRepository(Actor)
            .find({
                where: fullName.map(value => {
                    let nameAndSurname = value.split(" ");

                    return {
                        name: nameAndSurname[0],
                        surname: nameAndSurname[1] ?? ""
                    }
                })
            });
    }
}