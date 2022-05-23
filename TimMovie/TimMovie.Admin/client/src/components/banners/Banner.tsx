import {BannerDto} from "../../dto/BannerDto";
import styles from "./banner.module.css"

interface Props {
    banners: BannerDto[]
}

function Banners(props: Props) {
    return (
        <div>
            {
                props.banners.map((banner) =>
                    <div className={styles.bannerImage} style={{backgroundImage: `url(${process.env.REACT_APP_FILESERVER + banner.image})`}}>
                        <div className={styles.bannerContainer}>
                                <h1 className={styles.bannerFilmTitle}>{banner.filmTitle}</h1>
                                <p className={styles.bannerFilmDescription}>{banner.description}</p>
                        </div>
                    </div>
                )
            }
        </div>
    )
}

export default Banners