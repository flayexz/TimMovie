using FilesApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services
    .AddScoped<ImageService>()
    .AddScoped<FileService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();