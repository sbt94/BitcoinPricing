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


# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /app

# # Copy csproj and restore dependencies
# COPY *.csproj .
# RUN dotnet restore

# # Copy the rest and build
# COPY . .
# RUN dotnet publish -c Release -o out

# # Build runtime image
# FROM mcr.microsoft.com/dotnet/runtime:7.0
# WORKDIR /app
# COPY --from=build /app/out .
# ENTRYPOINT ["dotnet", "ServiceA.dll"]

# FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
# WORKDIR /app
# COPY . .
# ENTRYPOINT ["dotnet", "ServiceA.dll"]