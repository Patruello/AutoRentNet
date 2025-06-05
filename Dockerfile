# ---------- build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln .
COPY *.csproj ./

COPY tests/AutoRentNet.Tests/*.csproj ./tests/AutoRentNet.Tests/
RUN dotnet restore

COPY . .
RUN rm -rf tests

RUN dotnet publish AutoRentNet.csproj -c Release -o /app/publish

# ---------- runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ConnectionStrings__Default="Data Source=autorent.db"

EXPOSE 8080
ENTRYPOINT ["dotnet", "AutoRentNet.dll"]



