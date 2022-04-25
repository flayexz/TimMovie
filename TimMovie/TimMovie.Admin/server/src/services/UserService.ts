import {Injectable} from '@nestjs/common';
import {getRepository, In, Like, Raw} from "typeorm";
import {AspNetUsers} from "../../entities/AspNetUsers";
import {IUserDto} from "../dto/IUserDto";

@Injectable()
export class UserService {
    async getUsersWithFilterByLogin(incomingText: string, skip: number, take: number): Promise<IUserDto[]>{
        let lowerText = incomingText.toLowerCase();
        
        const userRepository = getRepository(AspNetUsers);
        let users = await userRepository.find({
            where: {
                userName: Raw(alias => `lower(${alias}) LIKE '%${lowerText}%'`),
            },
            relations: ["aspNetUserClaims","userSubscribes", "userSubscribes.subscribe"],
            skip,
            take
        });
        
        let usersDto: IUserDto[] = users.map(user => {
            return {
                id: user.id,
                login: user.userName,
                email: user.email,
                roles: user.aspNetUserClaims
                    .filter(claim => claim.claimType === "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    .map(claim => claim.claimValue),
                subscribes: user.userSubscribes.map(sub => sub.subscribe.name)
            }
        });
        return usersDto;
    }
}
