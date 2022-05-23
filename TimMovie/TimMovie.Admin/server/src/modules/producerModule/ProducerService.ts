import {Injectable} from "@nestjs/common";
import NameDto from "../../dto/NameDto";
import {getRepository} from "typeorm";
import {includeNamePart} from "../../common/queryFunction";
import {Producer} from "../../../entities/Producer";

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
}