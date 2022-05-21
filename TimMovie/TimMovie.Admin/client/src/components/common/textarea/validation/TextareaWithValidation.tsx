import messageErrorClasses from "../../css/messageError.module.css"
import React from "react";
import ValidationTextareaProps from "./ValidationTextareaProps";

function TextareaWithValidation({inputInfo, label, ...props}: ValidationTextareaProps){
    return (
        <>
            <div>{label}</div>
            <textarea onChange={inputInfo.onChange} onBlur={inputInfo.onBlur} value={inputInfo.value} 
                      className={props.inputClasses}/>
            {(inputInfo.isDirty && !inputInfo.validationState.inputIsValid) &&
                <div className={messageErrorClasses.messageError}>{inputInfo.validationState.errorMessage}</div>}
        </>
    );
}

export default TextareaWithValidation;