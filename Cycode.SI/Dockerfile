#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Cycode.SI/Cycode.SI.csproj", "Cycode.SI/"]
RUN dotnet restore "Cycode.SI/Cycode.SI.csproj"
COPY . .
WORKDIR "/src/Cycode.SI"
RUN dotnet build "Cycode.SI.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Cycode.SI.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY "Cycode.SI/run.sh" "/app/run.sh"
COPY --from=publish /app/publish .
ENTRYPOINT ["/bin/bash", "/app/run.sh"]

#