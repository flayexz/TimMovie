import styles from "../common/css/adminStyles.module.css";
import React from "react";

function AddBannerInputs() {
    return (<>
        <div className="mt-4 w-100 d-flex flex-row justify-content-around">
            <div className="d-flex flex-column">
                <label htmlFor="bannerDescriptionInput" className="text-center"><h5>Описание</h5></label>
                <textarea maxLength={250} id="bannerDescriptionInput" style={{resize: "none"}}
                          className={styles.adminInput} cols={60} rows={5}/>
            </div>
            <div className="d-flex flex-column">
                <label htmlFor="bannerSelectFilm"><h5>Фильм</h5></label>
                <select name="" id="bannerSelectFilm">
                    <option value="1">1</option>
                    <option value="2">2</option>
                </select>
            </div>
        </div>
    </>)
}

export default AddBannerInputs
