import {IShortUserInfoDto} from "./IShortUserInfoDto";

export interface IFullUserInfoDto extends IShortUserInfoDto{
    displayName: string;
    registrationDate: Date;
    birthDate: string;
    countryName: string | null;
}