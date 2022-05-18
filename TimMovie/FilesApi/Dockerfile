FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FilesApi.csproj", "FilesApi/"]
RUN dotnet restore "FilesApi/FilesApi.csproj" -r linux-arm64
COPY . .
WORKDIR "/src/FilesApi"
RUN dotnet build "FilesApi.csproj" -c Release -o /app/build -r linux-arm64

FROM build AS publish
RUN dotnet publish "FilesApi.csproj" -c Release -o /app/publish -r linux-arm64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FilesApi.dll"]
