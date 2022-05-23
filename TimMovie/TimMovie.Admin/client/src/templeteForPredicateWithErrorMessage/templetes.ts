import PredicateWithError from "../hook/validation/PredicateWithError";

export const checkOnEmpty: PredicateWithError<string> = { 
    valueIsValid: value => !!value,
    errorMessage: "Это поле должно быть заполнено"
} 