FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
MAINTAINER alsetor@gmail.com

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get install -y nodejs && \
    node -v && npm -v

COPY ["Application/Application.csproj", "Web/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Web/Web.csproj", "Web/"]

RUN dotnet restore "Web/Web.csproj"
COPY . .

WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

ENV NODE_OPTIONS=--openssl-legacy-provider

RUN dotnet publish "Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Web.dll"]