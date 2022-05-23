import messageErrorClasses from "../../css/messageError.module.css"
import React from "react";
import ValidationInputProps from "./ValidationInputProps";

function InputWithValidation({inputInfo, label, ...props}: ValidationInputProps){
    return (
        <>
            <div>{label}</div>
            <input onChange={inputInfo.onChange} onBlur={inputInfo.onBlur}
                   value={inputInfo.value} type={props.typeInput}  
                   className={props.inputClasses}/>
            {(inputInfo.isDirty && !inputInfo.validationState.inputIsValid) &&
                <div className={messageErrorClasses.messageError}>{inputInfo.validationState.errorMessage}</div>}
        </>  
    );
}

export default InputWithValidation;