import PredicateWithError from "./PredicateWithError";

export default interface Validation<TValue>{
    readonly predicates?: PredicateWithError<TValue>[]
}