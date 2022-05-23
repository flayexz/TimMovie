import React, {useEffect, useState} from "react";
import {UploadProps} from "../common/upload/UploadProps";
import {UploadFiles} from "../common/upload/UploadFiles";
import AddBannerInputs from "./AddBannerInputs";
import $api from "../../http";
import {BannerDto} from "../../dto/BannerDto";
import Banners from "./Banner";
import styles from "./banner.module.css";

function BannersPage(uploadProps: UploadProps) {

    const [preview, setPreview] = useState<string | null>(null);
    const [description, setDescription] = useState<string | null>(null)
    const [file, setFile] = useState<File[]>([]);
    const [banners, setBanners] = useState<Array<BannerDto>>([])

    useEffect(() => {
        let url = '/banners/getAllBanners'
        $api.get(url).then(response => setBanners([...banners, ...response.data]))
    }, [])

    return (<>
        <div className="justify-content-center d-flex flex-column align-items-center">
            <h1 className="mt-4">Добавить новый баннер</h1>
            <div className="mt-2 position-relative text-break">
                <UploadFiles uploadProps={uploadProps}
                             uploadHooks={{preview: preview, setPreview: setPreview, file: file, setFile: setFile}}/>
                {description && preview ? <div className={styles.bannerContainer}>
                    <h1 className={styles.bannerFilmTitle}>Название типа</h1>
                    <p className={styles.bannerFilmDescription}>{description}</p>
                </div> : ''}
            </div>
            <AddBannerInputs preview={preview} description={description} setDescription={setDescription}/>
            <hr className="w-100"/>
            <Banners banners={banners}/>
        </div>
    </>)
}

export default BannersPage