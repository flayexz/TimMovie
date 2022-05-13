import React, {useState} from "react";
import {UploadProps} from "../common/upload/UploadProps";
import {UploadFiles} from "../common/upload/UploadFiles";

function BannersPage(uploadProps: UploadProps) {
    return (<>
        <div className="justify-content-center d-flex flex-column align-items-center">
            <h1 className="mt-4">Добавить новый баннер</h1>
            <div className="mt-2">
                <UploadFiles uploadProps={uploadProps}/>
            </div>
            <hr/>
        </div>
    </>)
}

export default BannersPage