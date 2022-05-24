import {Admin} from "../authModule/adminAuth";
import {Body, Controller, Delete, Get, Param, Post, Query, UploadedFile, UseInterceptors} from "@nestjs/common";
import {BannerService} from "./bannerService";
import {BannerDto} from "../../dto/BannerDto";
import {FileInterceptor} from "@nestjs/platform-express";
import {NewBannerDto} from "../../dto/NewBannerDto";
import {Result} from "../../dto/Result";


@Admin()
@Controller('banners')
export class BannerController {
    constructor( private readonly bannerService: BannerService) {
    }

    @Get('getAllBanners')
    public async GetAllBanners(): Promise<BannerDto[]> {
        return await this.bannerService.GetAllBanners();
    }

    @Post('add')
    @UseInterceptors(FileInterceptor('img'))
    async addNewBanner(@Body() newBanner:NewBannerDto, @UploadedFile() image: Express.Multer.File) : Promise<Result<string>>{
        if (image == null){
            return {
                success: false,
                textError: "Необходимо добавить обложку для фильма",
            }
        }

        return await this.bannerService.addNewBanner(newBanner, image);
    }

    @Delete('/:bannerId')
    async deleteBanner(@Param("bannerId") bannerId: string):Promise<Result<string>>{
        console.log(bannerId)
        return await this.bannerService.deleteBanner(bannerId)
    }
}
