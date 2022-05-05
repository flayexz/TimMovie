import {Injectable} from '@nestjs/common';
import {IUserRoleDto} from "../dto/IUserRoleDto";
import {getRepository} from "typeorm";
import {AspNetUserClaim} from "../../entities/AspNetUserClaim";
import {IRoleDto} from "../dto/IRoleDto";
import {AspNetUser} from "../../entities/AspNetUser";

@Injectable()
export class RoleService {
   public async getUserRolesAndAllRemaining(id: string): Promise<IUserRoleDto[]> {
       let allRoles = await getRepository(AspNetUserClaim)
           .createQueryBuilder()
           .addSelect("AspNetUserClaim.Id", "id")
           .addSelect("AspNetUserClaim.claimValue", "claimValue")
           .where(`AspNetUserClaim.claimType = :claimType`, {claimType: process.env.CLAIM_ROLE})
           .distinctOn(["AspNetUserClaim.claimValue"])
           .execute();
       console.log(allRoles);
       let userRoles = await this.getAllUserRoles(id);
       
       let roles: IUserRoleDto[] = allRoles.map(role => {
          return  {
              role: {
                  id: role.id,
                  roleName: role.claimValue
              },
              userIsIncludedInRole: userRoles.find(userRole => userRole.roleName === role.claimValue) != undefined
          }
       });
       
       return roles;
   }
   
   public async getAllUserRoles(id: string): Promise<IRoleDto[]>{
       let user = await getRepository(AspNetUser)
           .findOne({
              where: {
                  id: id,
              },
               relations: ["aspNetUserClaims"]
           });
       
       let userRoles: IRoleDto[] = user.aspNetUserClaims
           .filter(claim => claim.claimType === process.env.CLAIM_ROLE)
           .map(claim => {
               return {
                   id: claim.id,
                   roleName: claim.claimValue
               }
           });
       return userRoles;
   }

    public async updateRolesForUser(userId: string, roleNames: string[]): Promise<void>{
        await getRepository(AspNetUserClaim)
            .delete({
                claimType: process.env.CLAIM_ROLE,
                userId: userId
            });

        await getRepository(AspNetUserClaim)
            .insert(roleNames.map(roleName => {
                return {
                    claimType: process.env.CLAIM_ROLE,
                    claimValue: roleName,
                    userId: userId
                }
            }));
    }
}
