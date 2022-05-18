import { Module } from '@nestjs/common';
import {SubscribeController} from "./subscribe.controller";
import {SubscribeService} from "./SubscribeService";

@Module({
    controllers: [SubscribeController],
    providers: [SubscribeService],
    exports: [SubscribeService]
})
export class SubscribeModule {}
