import {Injectable} from '@nestjs/common';
import {IUserRoleDto} from "../dto/IUserRoleDto";
import {getRepository} from "typeorm";
import {AspNetUserClaim} from "../../entities/AspNetUserClaim";
import {UserSubscribe} from "../../entities/UserSubscribe";
import {Guid} from "guid-typescript";
import {AspNetRoleClaim} from "../../entities/AspNetRoleClaim";

@Injectable()
export class RoleService {
   // async GetAllRolesWith(id: string): Promise<IUserRoleDto[]> {
   //     let roles = getRepository(AspNetUserClaim).find({
   //        
   //     });
   // }

    public async updateRolesForUser(userId: string, roleNames: string[]): Promise<void>{
        await getRepository(AspNetUserClaim)
            .delete({
                claimType: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                userId: userId
            });

        await getRepository(AspNetUserClaim)
            .insert(roleNames.map(roleName => {
                return {
                    claimType: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                    claimValue: roleName,
                    userId: userId
                }
            }));
    }
}
