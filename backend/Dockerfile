# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build


WORKDIR /src

COPY Sazzle.slnx ./
COPY Sazzle.Api/Sazzle.Api.csproj Sazzle.Api/
COPY Sazzle.Application/Sazzle.Application.csproj Sazzle.Application/
COPY Sazzle.Domain/Sazzle.Domain.csproj Sazzle.Domain/
COPY Sazzle.Infrastructure/Sazzle.Infrastructure.csproj Sazzle.Infrastructure/
COPY Sazzle.Tests/Sazzle.Tests.csproj Sazzle.Tests/

RUN dotnet restore Sazzle.slnx

COPY . .

RUN dotnet publish Sazzle.Api/Sazzle.Api.csproj \
-c Release \
-o /app/publish \
--no-restore



FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080


COPY --from=build /app/publish .

USER app

ENTRYPOINT ["dotnet", "Sazzle.Api.dll"]