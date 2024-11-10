FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ServiceA.csproj", "."]
RUN dotnet restore "ServiceA.csproj"
COPY . .
RUN dotnet publish "ServiceA.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ServiceA.dll"]