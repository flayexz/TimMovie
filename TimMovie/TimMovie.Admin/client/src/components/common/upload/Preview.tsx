import React, {FC, useRef, useState} from "react";
import styles from './styles/preview.module.css'

interface Props{
    setPreview: Function,
    preview: string
}

export const Preview: FC<Props> = ({setPreview, preview}) => {

    function removePreview() {
        setPreview(null)
    }

    return(<>
        <div>
            <img src={preview} alt="" className={styles.preview}/>
            <input type="button" className="btn btn-outline-danger w-100 mt-2" value="Отмена" onClick={removePreview} />
        </div>
    </>)
}