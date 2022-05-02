export interface IShortInformationAboutUserDto {
    id: string;
    login: string | null;
    email: string | null;
    roles: string[];
    subscribes: string[];
}