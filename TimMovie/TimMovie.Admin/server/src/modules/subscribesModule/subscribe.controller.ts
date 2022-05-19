import {Controller, Get, Query} from '@nestjs/common';
import {SubscribeService} from "./SubscribeService";
import {Admin} from "../authModule/adminAuth";

@Admin()
@Controller('subscribes')
export class SubscribeController {
    constructor(private readonly subscribeService: SubscribeService) {}
    
    @Get("collection")
    async GetUserSubscribesAndAllRemaining(@Query("userId") userId){
        let subscribes = await this.subscribeService.getUserSubscribesAndAllRemaining(userId);
        return subscribes;
    }
}
