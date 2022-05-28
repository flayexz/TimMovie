namespace TimMovie.WebApi.Tests

open TimMovie.Infrastructure.Database

type DatabaseFillerCommon() =
    static member Start(dbContext: ApplicationContext) =
        let dbFillerNavbarSearch = DatabaseFillerNavbarSearch()
        dbFillerNavbarSearch.Start(dbContext)
        ignore
