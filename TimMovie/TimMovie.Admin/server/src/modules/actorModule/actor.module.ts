import { Module } from '@nestjs/common';
import {ActorController} from "./actor.controller";
import {ActorService} from "./ActorService";

@Module({
    controllers: [ActorController],
    providers: [ActorService],
    exports: [ActorService]
})
export class ActorModule {}