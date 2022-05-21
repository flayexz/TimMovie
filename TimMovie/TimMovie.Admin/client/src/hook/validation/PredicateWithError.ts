export default interface PredicateWithError<TValue>{
    readonly valueIsValid: ((value: TValue) => boolean)
    readonly errorMessage?: string;
}