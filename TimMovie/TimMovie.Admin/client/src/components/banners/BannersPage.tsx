import React, {useState} from "react";
import {UploadProps} from "../common/upload/UploadProps";
import {UploadFiles} from "../common/upload/UploadFiles";
import AddBannerInputs from "./AddBannerInputs";

function BannersPage(uploadProps: UploadProps) {
    const [preview, setPreview] = useState<string | null>(null);
    const [file, setFile] = useState<File[]>([]);
    return (<>
        <div className="justify-content-center d-flex flex-column align-items-center">
            <h1 className="mt-4">Добавить новый баннер</h1>
            <div className="mt-2">
                <UploadFiles uploadProps={uploadProps} uploadHooks={{preview:preview, setPreview: setPreview, setFile: setFile}}/>
            </div>
            {preview ? <AddBannerInputs/> : ''}
            <hr className="w-100"/>
        </div>
    </>)
}

export default BannersPage