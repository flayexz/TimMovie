import {Controller, Get, Query} from '@nestjs/common';
import {ActorService} from "./ActorService";
import NameDto from "../../dto/NameDto";
import {Admin} from "../authModule/adminAuth";

@Admin()
@Controller('actors')
export class ActorController {
    constructor(private readonly actorService: ActorService) {
    }
    
    @Get("collection")
    async getActorsByNamePart(
        @Query("namePart") namePart: string,
        @Query("skip") skip:number,
        @Query("take") take: number): Promise<NameDto[]>{
        return await this.actorService.getActorsByNamePart(namePart, skip, take);
    }
}
