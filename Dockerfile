FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY KyrgyzTest.sln .
COPY KyrgyzTest.Core/KyrgyzTest.Core.csproj             KyrgyzTest.Core/
COPY KyrgyzTest.Application/KyrgyzTest.Application.csproj KyrgyzTest.Application/
COPY KyrgyzTest.Infrastructure/KyrgyzTest.Infrastructure.csproj KyrgyzTest.Infrastructure/
COPY KyrgyzTest.API/KyrgyzTest.API.csproj               KyrgyzTest.API/
COPY KyrgyzTest.Tests/KyrgyzTest.Tests.csproj           KyrgyzTest.Tests/

RUN dotnet restore

COPY . .
RUN dotnet publish KyrgyzTest.API -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

RUN mkdir -p uploads

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "KyrgyzTest.API.dll"]
