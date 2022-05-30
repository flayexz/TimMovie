namespace TimMovie.WebApi.Tests.Helpers

open System

[<CustomComparison; CustomEquality>]
type FilmFilterHelper =
    { Id : Guid;
      Title : string;
      Year  : int;
      Rating : Nullable<double>;}
    interface IComparable<FilmFilterHelper> with
        member this.CompareTo(other) =
            this.Title.CompareTo other.Title


              
    interface IEquatable<FilmFilterHelper> with
        member this.Equals { Id = id } =
            this.Id = id        
    override this.Equals obj =
        match obj with
          | :? FilmFilterHelper as other -> (this :> IEquatable<_>).Equals other
          | _ -> false
