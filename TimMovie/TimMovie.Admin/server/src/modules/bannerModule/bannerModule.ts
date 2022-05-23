import {Module} from "@nestjs/common";
import {BannerController} from "./bannerController";
import {BannerService} from "./bannerService";

@Module({
    controllers: [BannerController],
    providers: [BannerService]
})
export class BannerModule {}