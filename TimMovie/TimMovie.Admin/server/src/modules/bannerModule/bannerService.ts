import {Injectable} from "@nestjs/common";
import {getRepository} from "typeorm";
import {BannerDto} from "../../dto/BannerDto";
import {Banner} from "../../../entities/Banner";
import {Film} from "../../../entities/Film";
import {relationName} from "typeorm-model-generator/dist/src/NamingStrategy";

@Injectable()
export class BannerService{
    public async GetAllBanners(): Promise<BannerDto[]>{
        const banners = await getRepository(Banner).find({relations:['film']})
        return banners.map(banner => {
            return {
                description: banner.description,
                image: banner.image,
                filmTitle: banner.film.title
            }
        })
    }
}