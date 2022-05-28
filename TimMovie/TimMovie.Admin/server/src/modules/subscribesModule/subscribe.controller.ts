import {Controller, Get} from '@nestjs/common';
import {SubscribeService} from "./SubscribeService";
import {Admin} from "../authModule/adminAuth";
import NameDto from "../../dto/NameDto";

@Admin()
@Controller('subscribes')
export class SubscribeController {
    constructor(private readonly subscribeService: SubscribeService) {}
    
    @Get("collection")
    async getAllSubscribes(): Promise<NameDto[]>{
        return await this.subscribeService.getAllSubscribes();
    }
}
