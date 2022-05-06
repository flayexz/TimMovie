import {Injectable} from '@nestjs/common';
import {getRepository, Raw} from "typeorm";
import {AspNetUser} from "../../entities/AspNetUser";
import {IShortInformationAboutUserDto} from "../dto/IShortInformationAboutUserDto";
import {IAllInformationAboutUserDto} from "../dto/IAllInformationAboutUserDto";

@Injectable()
export class UserService {
    public async getUsersWithFilterByLogin(incomingText: string, skip: number, take: number): Promise<IShortInformationAboutUserDto[]>{
        let lowerText = incomingText.toLowerCase();
        
        const userRepository = getRepository(AspNetUser);
        let users = await userRepository.find({
            where: {
                userName: Raw(alias => `lower(${alias}) LIKE '%${lowerText}%'`),
            },
            relations: ["aspNetUserClaims","userSubscribes", "userSubscribes.subscribe"],
            skip,
            take
        });
        
        let usersDto: IShortInformationAboutUserDto[] = users.map(user => {
            return {
                id: user.id,
                login: user.userName,
                email: user.email,
                roles: user.aspNetUserClaims
                    .filter(claim => claim.claimType === process.env.CLAIM_ROLE)
                    .map(claim => claim.claimValue),
                subscribes: user.userSubscribes.map(sub => sub.subscribe.name)
            }
        });
        return usersDto;
    }
    
    public async getAllInfoAboutUser(id: string): Promise<IAllInformationAboutUserDto>{
        const userRepository = getRepository(AspNetUser);
        let user = await userRepository.findOne({
            where: {
                id : id,
            },
            relations: ["aspNetUserClaims","userSubscribes", "userSubscribes.subscribe", "country"],
        });
        
        if (user == null){
            return null;
        }

        let userDto: IAllInformationAboutUserDto = {
            id: user.id,
            login: user.userName,
            email: user.email,
            roles: user.aspNetUserClaims
                .filter(claim => claim.claimType === process.env.CLAIM_ROLE)
                .map(claim => claim.claimValue),
            subscribes: user.userSubscribes.map(sub => sub.subscribe.name),
            displayName: user.displayName,
            registrationDate: user.registrationDate,
            birthDate: user.birthDate,
            countryName: user.country?.name ?? null,
        };
        
        return userDto; 
    }
    
    public async userIsExisted(id: string): Promise<boolean>{
        let user = await getRepository(AspNetUser)
            .findOne({where: {id: id}});
        return user != undefined;
    }
}
