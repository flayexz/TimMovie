import { IShortInformationAboutUserDto } from './IShortInformationAboutUserDto';

export interface IAllInformationAboutUserDto
  extends IShortInformationAboutUserDto {
  displayName: string;
  registrationDate: Date;
  birthDate: string;
  countryName: string | null;
}
