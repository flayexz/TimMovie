namespace TimMovie.WebApi.Tests.Extensions.LinqExtensions

open System

type LinqHelper() =
    static member IsSorted<'T when 'T :> IComparable and 'T: equality and 'T: null>
        (enumerable: System.Collections.Generic.IEnumerable<'T>)
        =
        let mutable prev = Unchecked.defaultof<'T>
        let mutable prevSet = false
        let mutable result = true

        for el in enumerable do
            if (prevSet && (prev = null || prev.CompareTo(el) > 0)) then
                result <- false
            else
                prev <- el
                prevSet <- true

        result
