import React, {FC, useRef, useState} from "react";
import styles from './styles/preview.module.css'
import {UploadProps} from "./UploadProps";

interface Props{
    setPreview: Function,
    preview: string,
    uploadProps: UploadProps
}

export const Preview: FC<Props> = ({setPreview, preview,uploadProps}) => {

    function removePreview() {
        setPreview(null)
    }

    return(<>
        <div className="d-flex flex-column align-items-center">
            <img src={preview} alt="" className={styles.preview} width={uploadProps.photoWidth} height={uploadProps.photoHeight} style={{borderRadius: uploadProps.borderRadius}}/>
            <input type="button" className="btn btn-outline-danger w-100 mt-2" value="Отмена" onClick={removePreview} />
        </div>
    </>)
}