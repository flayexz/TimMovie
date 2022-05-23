import {Controller, Get, Query} from '@nestjs/common';
import NameDto from "../../dto/NameDto";
import {ProducerService} from "./ProducerService";

@Controller('producers')
export class ProducerController {
    constructor(private readonly producerService: ProducerService) {
    }

    @Get("collection")
    async getProducersByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.producerService.getActorsByNamePart(namePart, skip, take);
    }
}
