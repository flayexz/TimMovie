import { Controller, Get } from '@nestjs/common';
import { AppService } from './app.service';
import {Banners} from '../entities/Banners';
import {getRepository} from "typeorm"


@Controller()
export class AppController {
  constructor(private readonly appService: AppService) {}

  @Get('banners')
  async getBanners(): Promise<Banners[]> {
    const bannerRepository = getRepository(Banners)
    return await bannerRepository.find()
  }
}
