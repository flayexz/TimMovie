import {Admin} from "../authModule/adminAuth";
import {Controller, Get} from "@nestjs/common";
import {BannerService} from "./bannerService";
import {BannerDto} from "../../dto/BannerDto";


@Admin()
@Controller('banners')
export class BannerController {
    constructor( private readonly bannerService: BannerService) {
    }

    @Get('getAllBanners')
    public async GetAllBanners(): Promise<BannerDto[]> {
        return await this.bannerService.GetAllBanners();
    }
}
