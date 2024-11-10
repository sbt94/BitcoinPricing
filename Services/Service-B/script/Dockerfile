FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ServiceB.csproj", "."]
RUN dotnet restore "ServiceB.csproj"
COPY . .
RUN dotnet publish "ServiceB.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ServiceB.dll"]

# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /app
# COPY . .
# RUN dotnet publish -c Release -o out

# FROM mcr.microsoft.com/dotnet/aspnet:7.0
# WORKDIR /app
# COPY --from=build /app/out .
# ENTRYPOINT ["dotnet", "ServiceA.dll"]


# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# WORKDIR /app
# COPY . .
# ENTRYPOINT ["dotnet", "ServiceB.dll"]