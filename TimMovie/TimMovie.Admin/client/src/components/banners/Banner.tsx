import {BannerDto} from "../../dto/BannerDto";
import styles from "./banner.module.css"
import $api from "../../http";

interface Props {
    banners: BannerDto[],
    setIsSubmitted: Function
}


function Banners(props: Props) {

    function onDelete(bannerId: string) {
        let formData = new FormData()
        formData.append('bannerId', bannerId)
        $api.delete(`/banners/${bannerId}`).then(response => {
            if (response.data.success) {
                props.setIsSubmitted(true)
                alert('баннер удален')
            } else {
                alert(response.data.textError)
            }
        })
    }

    return (
        <div>
            {
                props.banners.map((banner) =>
                    <div className={styles.bannerImage}
                         style={{backgroundImage: `url(${process.env.REACT_APP_FILESERVER + banner.image})`}}>
                        <div className={styles.bannerContainer}>
                            <h1 className={styles.bannerFilmTitle}>{banner.filmTitle}</h1>
                            <p className={styles.bannerFilmDescription}>{banner.description}</p>
                            <input type="button" className="btn btn-danger btn-lg" value="Удалить" onClick={() => onDelete(banner.bannerId)}/>
                        </div>
                    </div>
                )
            }
        </div>
    )
}

export default Banners