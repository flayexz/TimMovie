import {BannerDto} from "../../dto/BannerDto";

interface Props {
    banners: BannerDto[]
}

function Banners(props: Props) {
    return (
        <div>
            {
                props.banners.map((banner) =>
                    <div style={{
                        marginTop:"15px",
                        borderRadius: "40px",
                        height: "512px",
                        width: "1100px",
                        backgroundPosition: "center",
                        backgroundSize: "cover",
                        backgroundImage: `url(${process.env.REACT_APP_FILESERVER + banner.image})`,
                        position: 'relative',
                        opacity:'0.85'
                    }}>
                        <div style={{
                            width: "90%",
                            bottom: "13%",
                            left: 0,
                            right: 0,
                            paddingBottom: "20px",
                            color: "white",
                            opacity:"1 !important",
                            marginLeft: "15px",
                            position: "absolute",
                        }}>
                            <div style={{
                                width: "90%",
                            }}>
                                <h1 style={{fontSize:'3rem'}}>{banner.filmTitle}</h1>
                                <p style={{fontSize:'20px'}}>{banner.description}</p>
                            </div>
                        </div>
                    </div>
                )
            }
        </div>
    )
}

export default Banners