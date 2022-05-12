import React, {useState} from "react";
import {FileInput} from "./FileInput";
import {Preview} from "./Preview";

function UploadFiles(){
    const [preview, setPreview] = useState(null);
    const [file, setFile] = useState([]);
    return (
        <>
            {
                preview ? <Preview setPreview={setPreview} preview={preview} /> : <FileInput setPreview={setPreview} setFile={setFile}/>
            }
        </>
    );
}

export default UploadFiles