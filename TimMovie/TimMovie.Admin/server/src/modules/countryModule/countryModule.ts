import { Module } from '@nestjs/common';
import { CountryController } from './countryController';
import {CountryService} from "./country.service";

@Module({
  controllers: [CountryController],
  providers: [CountryService]
})
export class CountryModule {}
