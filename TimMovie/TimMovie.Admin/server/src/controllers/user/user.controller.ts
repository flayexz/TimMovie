import {Controller, Get, Query} from '@nestjs/common';
import {IUserDto} from 'src/dto/IUserDto';
import {UserService} from "../../services/UserService";
import {Admin} from "../../auth/adminAuth";

@Admin()
@Controller('users')
export class UserController {
    constructor(private readonly userService: UserService) {}

    @Get('collection')
    async getUsersWithFilterByLogin(
            @Query("incomingText") incomingText: string, 
            @Query("skip") skip: number, 
            @Query("take") take: number): Promise<IUserDto[]> {
        console.log(`${skip} ${take} ${incomingText}`);
        
        const users = await this.userService.getUsersWithFilterByLogin(incomingText, skip, take);
        return users;
    }
}
