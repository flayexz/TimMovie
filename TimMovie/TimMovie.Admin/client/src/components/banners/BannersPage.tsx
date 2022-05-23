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
    const [description, setDescription] = useState<string>('')
    const [file, setFile] = useState<File>()
    const [banners, setBanners] = useState<Array<BannerDto>>([])
    const [film, setFilm] = useState()
    const [isSubmitted, setIsSubmitted] = useState<boolean>(true)

    useEffect(() => {
        if(isSubmitted){
            initBanners()
            setIsSubmitted(false)
        }
    }, [isSubmitted])

    function initBanners(){
        let url = '/banners/getAllBanners'
        $api.get(url).then(response => setBanners([...response.data]))
    }

    async function trySaveBanner(): Promise<void> {
        if(!description){
            return;
        }
        let data = generateRequestData()
        $api.post('/banners/add', data).then(response => {
            if(!response.data.success){
                alert(response.data.textError ?? 'произошла ошибка')
            }
            else{
                setIsSubmitted(true)
                resetFields()
                alert('баннер успешно добавлен')
            }
        })
    }

    function resetFields(){
        setDescription('')
        setPreview(null)
        setFile(undefined)
    }

    function generateRequestData(){
        let formData = new FormData()
        formData.append("img", file!);
        formData.append("description", description!);
        formData.append("filmTitle", "Гери");
        return formData
    }

    return (<>
        <div className="justify-content-center d-flex flex-column align-items-center">
            <h1 className="mt-4">Добавить новый баннер</h1>
            <div className="mt-2 position-relative text-break">
                <UploadFiles uploadProps={uploadProps}
                             uploadHooks={{preview: preview, setPreview: setPreview, setFile: setFile}}/>
                {description && preview ? <div className={styles.bannerContainer} style={{position:"absolute",bottom:"13%"}}>
                    <h1 className={styles.bannerFilmTitle}>Название типа</h1>
                    <p className={styles.bannerFilmDescription}>{description}</p>
                    <input type="button" className="btn btn-lg btn-success" value="Сохранить" onClick={trySaveBanner}/>
                </div> : ''}
            </div>
            <AddBannerInputs preview={preview} description={description} setDescription={setDescription}/>
            <hr className="w-100"/>
            <Banners setIsSubmitted={setIsSubmitted} banners={banners}/>
        </div>
    </>)
}

export default BannersPage