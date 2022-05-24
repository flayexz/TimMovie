import {Injectable} from "@nestjs/common";
import {getRepository} from "typeorm";
import {BannerDto} from "../../dto/BannerDto";
import {Banner} from "../../../entities/Banner";
import {Film} from "../../../entities/Film";
import {relationName} from "typeorm-model-generator/dist/src/NamingStrategy";
import {NewBannerDto} from "../../dto/NewBannerDto";
import {Result} from "../../dto/Result";
import {Guid} from "guid-typescript";
import {FileService} from "../FileService";
import PaginationLoading from "../../dto/PaginationLoading";
import FilmForTableDto from "../../dto/FilmForTableDto";
import {includeNamePart} from "../../common/queryFunction";
import {plainToInstance} from "class-transformer";

@Injectable()
export class BannerService {
    constructor(private readonly fileService: FileService) {
    }

    public async GetAllBanners(): Promise<BannerDto[]> {
        const banners = await getRepository(Banner).find({relations: ['film']})
        return banners.map(banner => {
            return {
                description: banner.description,
                image: banner.image,
                filmTitle: banner.film.title,
                bannerId: banner.id
            }
        })
    }

    async getBannersByPart(pagination: PaginationLoading): Promise<BannerDto[]> {
        let banners = await getRepository(Banner)
            .find({
                relations: ['film'],
                take: pagination.take,
                skip: pagination.skip
            });
        return banners.map(banner => {
            return{
                description: banner.description,
                image: banner.image,
                filmTitle: banner.film.title,
                bannerId: banner.id
            }
        })
    }

    public async addNewBanner(newBanner: NewBannerDto, image: Express.Multer.File): Promise<Result<string>> {
        let resultSaveImage = await this.fileService.saveImage(image, 'banner');

        if (!resultSaveImage.success) {
            return {
                success: false,
                textError: "Во время сохранения  произошла ошибка",
            }
        }

        const bannerRepository = getRepository(Banner)

        let banner = bannerRepository.create({
            id: Guid.create().toString(),
            description: newBanner.description,
            image: resultSaveImage.result
        })

        let film = await getRepository(Film)
            .findOne({
                where: {
                    title: newBanner.filmTitle
                }
            });

        if (film == null) {
            return {success: false, textError: 'не найден фильм'}
        }

        banner.film = film;
        try {
            await bannerRepository.save(banner);
            return {success: true}
        } catch (e) {
            return {success: false, textError: e.message}
        }
    }

    public async deleteBanner(bannerId: string): Promise<Result<string>> {
        try {
            const bannerRepository = getRepository(Banner)
            await bannerRepository.delete(bannerId)
            return {success: true}
        } catch (e) {
            return {success: false, textError: e.message}
        }
    }

}