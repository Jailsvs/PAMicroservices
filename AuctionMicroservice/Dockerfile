FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["AuctionMicroservice/AuctionMicroservice.csproj", "AuctionMicroservice/"]
COPY ["SharedMicroservice/SharedMicroservice.csproj", "SharedMicroservice/"]
RUN dotnet restore "AuctionMicroservice/AuctionMicroservice.csproj"
COPY . .
WORKDIR "/src/AuctionMicroservice"
RUN dotnet build "AuctionMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuctionMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuctionMicroservice.dll"]