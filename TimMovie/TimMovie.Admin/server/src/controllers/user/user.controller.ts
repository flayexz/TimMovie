import {Body, Controller, Get, Param, Post, Query} from '@nestjs/common';
import {IShortInformationAboutUserDto} from 'src/dto/IShortInformationAboutUserDto';
import {UserService} from "../../services/UserService";
import {IAllInformationAboutUserDto} from "../../dto/IAllInformationAboutUserDto";
import {SubscribeService} from "../../services/SubscribeService";
import {RoleService} from "../../services/RoleService";

@Controller('users')
export class UserController {
    constructor(private readonly userService: UserService,
                private readonly subscribeService: SubscribeService,
                private readonly roleService: RoleService) {}
    
    @Get('collection')
    public async getUsersWithFilterByLogin(
            @Query("incomingText") incomingText: string, 
            @Query("skip") skip: number, 
            @Query("take") take: number): Promise<IShortInformationAboutUserDto[]> {
        console.log(`${skip} ${take} ${incomingText}`);
        
        const users = await this.userService.getUsersWithFilterByLogin(incomingText, skip, take);
        return users;
    }

    @Get(':id')
    public async getAllInfoAboutUser(@Param('id') id: string): Promise<IAllInformationAboutUserDto | null> {
        const user: IAllInformationAboutUserDto = await this.userService.getAllInfoAboutUser(id);
        return user;
    }
    
    @Post("updateSubscribes")
    public async updateAllSubscribes(
        @Body("userId")userId: string, 
        @Body("subscribeIds")subscribeIds: string[]): Promise<void>{
        await this.subscribeService.updateSubscribeForUser(userId, subscribeIds);
    }

    @Post("updateRoles")
    public async updateAllRoles(
        @Body("userId")userId: string,
        @Body("roleNames")roleNames: string[]): Promise<void>{
        await this.roleService.updateRolesForUser(userId, roleNames);
    }
}
