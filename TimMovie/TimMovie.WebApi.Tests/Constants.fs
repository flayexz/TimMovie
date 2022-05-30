namespace TimMovie.WebApi.Tests

type Constants() =
    static member NavbarSearch = "/MainPage/SearchEntityByNamePart"
    static member Notifications = "/MainPage/GetAllUserNotifications"
    static member FilmByFilters = "/MainPage/GetFilmByFilters"
    static member AllUserSubscribes = "/MainPage/GetUserSubscribes"

    static member DefaultUserName = "testUser"
    static member DefaultDisplayName = Constants.DefaultUserName
    static member DefaultEmail = "testEmail@testEmail.testEmail"
    static member DefaultPathToPhoto = "123456"
    static member DefaultPassword = "testPassword111"
