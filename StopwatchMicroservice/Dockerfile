FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["StopwatchMicroservice/StopwatchMicroservice.csproj", "StopwatchMicroservice/"]
COPY ["SharedMicroservice/SharedMicroservice.csproj", "SharedMicroservice/"]
RUN dotnet restore "StopwatchMicroservice/StopwatchMicroservice.csproj"
COPY . .
WORKDIR "/src/StopwatchMicroservice"
RUN dotnet build "StopwatchMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StopwatchMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StopwatchMicroservice.dll"]