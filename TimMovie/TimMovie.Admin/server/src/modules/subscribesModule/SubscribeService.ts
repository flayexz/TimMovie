import {Injectable} from "@nestjs/common";
import {getRepository, In, Not} from "typeorm";
import {Guid} from "guid-typescript";
import {Subscribe} from "../../../entities/Subscribe";
import {UserSubscribe} from "../../../entities/UserSubscribe";
import NameDto from "../../dto/NameDto";
import {plainToInstance} from "class-transformer";
import {addMonths} from "../../common/dateExtensions";

@Injectable()
export class SubscribeService {
    public async getAllSubscribes(): Promise<NameDto[]>{
        let allSubscribes = await getRepository(Subscribe).find();
        let subscribesDto = plainToInstance(NameDto, allSubscribes);
        
        console.log(subscribesDto);
        
        return subscribesDto;
    }
    
    public async updateSubscribeForUser(userId: string, subscribeNames: string[]): Promise<void>{
        let subscribeIds = (await getRepository(Subscribe)
            .find({
                where: {
                    name: In(subscribeNames)
                }
            })).map(value => value.id);
        
        let userSubscribeRep = getRepository(UserSubscribe);
        
        let notChangedSubscribes = (await userSubscribeRep
            .find({
                where: {
                    subscribedUserId: userId,
                    subscribeId: In(subscribeIds)
                }
            })).map(value => value.subscribeId);

        let subscribeForAdd = subscribeIds.filter(id => !notChangedSubscribes.includes(id));
 
        
        await userSubscribeRep.delete({
            subscribedUserId: userId,
            subscribeId: Not(In(subscribeIds))
        });
        
        await userSubscribeRep
            .insert(subscribeForAdd.map(id => {
                return {
                    id: Guid.create().toString(),
                    subscribeId: id,
                    subscribedUserId: userId,
                    startDay: new Date(),
                    endDate: addMonths(new Date(), 1)
                }
            }));
    }
}
