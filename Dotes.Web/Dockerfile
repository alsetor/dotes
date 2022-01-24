#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
MAINTAINER stepanov_i@mail.ru

# Setup NANO and Last File Manager and Midnight Commander
RUN apt-get update && \
    apt-get install -y mc && \
    apt-get install -y nano && \
    apt-get install -y curl
# end setup

# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get install -y nodejs && \
    apt-get install -y build-essential
RUN npm install @angular/cli -g
RUN nodejs --version
# End setup

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build

RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
    apt-get install -y nodejs && \
    apt-get install -y build-essential
RUN npm install @angular/cli -g
RUN nodejs --version

WORKDIR /src
COPY ["Templates.Web/Templates.Web.csproj", "Templates.Web/"]
COPY ["Services/AuthService/AuthService.csproj", "Services/AASKNDService/"]
COPY ["Templates.BE/Templates.BE.csproj", "Templates/Templates.BE/"]
COPY ["Templates.BL/Templates.BL.csproj", "Templates/Templates.BL/"]
COPY ["Templates.DAL/Templates.DAL.csproj", "Templates/Templates.DAL/"]
RUN dotnet restore "Templates.Web/Templates.Web.csproj"
COPY . .
WORKDIR "/src/Templates.Web"
RUN dotnet build "Templates.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Templates.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
WORKDIR /app/ClientApp
RUN npm rebuild node-sass
WORKDIR /app
ENTRYPOINT ["dotnet", "Templates.Web.dll"]