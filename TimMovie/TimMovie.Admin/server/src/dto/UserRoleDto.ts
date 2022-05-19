import {RoleDto} from "./RoleDto";

export interface UserRoleDto {
    role: RoleDto;
    userIsIncludedInRole: boolean;
}