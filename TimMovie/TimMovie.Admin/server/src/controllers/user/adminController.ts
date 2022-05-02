import {Controller, Get, Query, UseGuards} from "@nestjs/common";
import {AdminAuth, Admin} from "../../auth/adminAuth";

@Controller('admin')
export class AdminController {

    @Admin()
    @Get('check')
    getUsersWithFilterByLogin(){
        return "ура!";
    }
}