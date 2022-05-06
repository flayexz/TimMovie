export interface IShortUserInfoDto {
    id: string;
    login: string | null;
    email: string | null;
    roles: string[];
    subscribes: string[];
}