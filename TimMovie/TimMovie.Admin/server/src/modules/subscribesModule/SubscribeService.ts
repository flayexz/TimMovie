import {Injectable} from "@nestjs/common";
import {getRepository} from "typeorm";
import {Guid} from "guid-typescript";
import {Subscribe} from "../../../entities/Subscribe";
import {UserSubscribeDto} from "../../dto/UserSubscribeDto";
import {UserSubscribe} from "../../../entities/UserSubscribe";
import {SubscribeDto} from "../../dto/SubscribeDto";

@Injectable()
export class SubscribeService {
    public async getUserSubscribesAndAllRemaining(userId: string): Promise<UserSubscribeDto[]>{
        let allSubscribes = await getRepository(Subscribe).find();
        let userSubscribes = await this.getAllUserSubscribes(userId); 
        let userSubscribesAndAllRemaining: UserSubscribeDto[] = allSubscribes.map(subscribe =>{
            return {
                subscribe: {
                    id: subscribe.id,
                    subscribeName: subscribe.name
                },
                userIsIncludedInSubscribe: userSubscribes
                    .find(value => value.subscribeName === subscribe.name) != undefined,
            }
        })
        
        return userSubscribesAndAllRemaining;
    }
    
    public async getAllUserSubscribes(userId: string): Promise<SubscribeDto[]>{
        let subscribes  = await getRepository(UserSubscribe).find({
            where:{ subscribedUserId: userId },
            relations: ["subscribe"],
        })
        
        let subscribesDto: SubscribeDto[] = subscribes.map(subscribe => {
           return {
               id: subscribe.subscribeId,
               subscribeName: subscribe.subscribe.name,
           } 
        });
        
        return subscribesDto;
    }
    
    public async updateSubscribeForUser(userId: string, subscribeIds: string[]): Promise<void>{
        await getRepository(UserSubscribe)
            .delete({
                subscribedUserId: userId
            });
        
        await getRepository(UserSubscribe)
            .insert(subscribeIds.map(subscribeId => {
                return {
                    id: Guid.create().toString(),
                    subscribedUserId: userId,
                    subscribeId: subscribeId,
                    startDay: new Date(),
                    endDate: new Date()
                }
            }));
    }
}
