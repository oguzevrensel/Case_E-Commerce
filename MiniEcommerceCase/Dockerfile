# Base image: .NET 8 ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image: .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["MiniEcommerceCase/MiniEcommerceCase.csproj", "MiniEcommerceCase/"]
COPY ["MiniEcommerceCase.Application/MiniEcommerceCase.Application.csproj", "MiniEcommerceCase.Application/"]
COPY ["MiniEcommerceCase.Domain/MiniEcommerceCase.Domain.csproj", "MiniEcommerceCase.Domain/"]
COPY ["MiniEcommerceCase.Infrastructure/MiniEcommerceCase.Infrastructure.csproj", "MiniEcommerceCase.Infrastructure/"]

RUN dotnet restore "MiniEcommerceCase/MiniEcommerceCase.csproj"

# Copy all source code
COPY . .
WORKDIR "/src/MiniEcommerceCase"
RUN dotnet build "MiniEcommerceCase.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "MiniEcommerceCase.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MiniEcommerceCase.dll"]
