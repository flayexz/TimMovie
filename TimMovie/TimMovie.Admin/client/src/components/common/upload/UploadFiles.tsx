import React, {FC, useState} from "react";
import {FileInput} from "./FileInput";
import {Preview} from "./Preview";
import {UploadProps} from "./UploadProps";

interface Props{
    uploadProps: UploadProps
}

export const UploadFiles: FC<Props> = ({uploadProps}) => {
    const [preview, setPreview] = useState(null);
    const [file, setFile] = useState([]);
    return (
        <>
            {
                preview ? <Preview uploadProps={uploadProps} setPreview={setPreview} preview={preview} /> : <FileInput uploadProps={uploadProps} setPreview={setPreview} setFile={setFile}/>
            }
        </>
    );
}