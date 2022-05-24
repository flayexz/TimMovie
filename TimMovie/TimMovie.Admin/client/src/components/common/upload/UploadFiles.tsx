import React, {FC} from "react";
import {FileInput} from "./FileInput";
import {Preview} from "./Preview";
import {UploadProps} from "./UploadProps";
import {UploadHooks} from "./UploadHooks";

interface Props {
    uploadProps: UploadProps,
    uploadHooks: UploadHooks
}

export const UploadFiles: FC<Props> = ({uploadProps, uploadHooks}) => {

    return (
        <>
            {
                uploadHooks.preview ? <Preview uploadProps={uploadProps} setPreview={uploadHooks.setPreview}
                                   preview={uploadHooks.preview} setFile={uploadHooks.setFile}/> :
                    <FileInput uploadProps={uploadProps} setPreview={uploadHooks.setPreview} setFile={uploadHooks.setFile}/>
            }
        </>
    );
}