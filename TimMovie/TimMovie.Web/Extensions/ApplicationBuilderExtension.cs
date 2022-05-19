using TimMovie.Web.Middleware;

namespace TimMovie.Web.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseFileService(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<FileServiceMiddleware>();
    }

    public static IApplicationBuilder UseUserStatusService(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserStatusServiceMiddleware>();
    }
}