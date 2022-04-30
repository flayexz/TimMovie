import {IRoleDto} from "./IRoleDto";

export interface IUserRoleDto{
    role: IRoleDto;
    userIsIncludedInRole: boolean;
}