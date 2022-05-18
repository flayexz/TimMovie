import {Controller, Get, Query} from '@nestjs/common';
import {RoleService} from "./RoleService";
import {Admin} from "../authModule/adminAuth";

@Admin()
@Controller('roles')
export class RoleController {
    constructor(private readonly roleService: RoleService) {
    }
    
    @Get("collectionForUser")
    async GetUserRolesAndAllRemaining(@Query("userId") userId){
        let subscribes = await this.roleService.getUserRolesAndAllRemaining(userId);
        
        return subscribes;
    }
}
