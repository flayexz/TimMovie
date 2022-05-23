import { Module } from '@nestjs/common';
import {ProducerController} from "./producer.controller";
import {ProducerService} from "./ProducerService";

@Module({
    controllers: [ProducerController],
    providers: [ProducerService]
})
export class ProducerModule {}