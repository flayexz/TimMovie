import {Injectable} from "@nestjs/common";
import NameDto from "../../dto/NameDto";
import {getRepository} from "typeorm";
import {includeNamePart} from "../../common/queryFunction";
import {Producer} from "../../../entities/Producer";
import {Actor} from "../../../entities/Actor";

@Injectable()
export class ProducerService{
    async getActorsByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]>{
        if (namePart == null){
            return [];
        }

        let producers = await getRepository(Producer)
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

        let producersDto = producers.map(producer => {
            return {
                id: producer.id,
                name: `${producer.name} ${producer.surname}`
            }
        });
        return producersDto;
    }

    async getProducersByFullName(fullName: string[]): Promise<Producer[]>{
        return await getRepository(Producer)
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