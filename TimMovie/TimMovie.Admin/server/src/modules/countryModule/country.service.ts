import {Injectable} from "@nestjs/common";
import NameDto from "../../dto/NameDto";
import {getRepository} from "typeorm";
import {includeNamePart} from "../../common/queryFunction";
import {Producer} from "../../../entities/Producer";
import {Country} from "../../../entities/Country";

@Injectable()
export class CountryService {
    async getCountriesByNamePart(namePart: string, skip: number, take: number): Promise<NameDto[]>{
        if (namePart == null){
            return [];
        }

        let countries = await getRepository(Country)
            .find({
                where: [
                    {name: includeNamePart(namePart)},
                ],
                take,
                skip,
                order: {
                    name: 'ASC',
                }
            });

        let countriesDto = countries.map(country => {
            return {
                id: country.id,
                name: country.name
            }
        });
        return countriesDto;
    }
}