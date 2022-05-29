namespace TimMovie.WebApi.Tests

type Constants() =
    static member NavbarSearch = "/MainPage/SearchEntityByNamePart"
    static member Authorization = "/connect/token"
    static member Registration = "/Account/Register"
    static member Notifications = "/MainPage/GetAllUserNotifications"
    static member UrlToConfirmEmail = "/Account/GetUrlToConfirmEmail"
    static member DefaultEmail = "testEmail@testEmail.testEmail"
    static member DefaultUserName = "testUser"
    static member DefaultPassword = "testPassword111"
