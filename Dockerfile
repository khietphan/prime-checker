FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY . ./
RUN dotnet restore PrimeChecker.sln  -nowarn:msb3202,nu1503

COPY . .
WORKDIR ./PrimeChecker.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app

RUN openssl genrsa -aes256 -passout pass:P@ssw0rd -out server.key 4096
RUN openssl rsa -in server.key -out server.key.insecure -passin pass:P@ssw0rd
RUN rm server.key
RUN openssl req -new -newkey rsa:4096 -x509 -nodes -days 3650 -keyout server.key -out server.crt -subj "/C=VN/ST=HCMC/L=HCMC /O=Khiet/OU=Internal/CN=localhost" -passin pass:P@ssw0rd
RUN openssl pkcs12 -export -out localhost.pfx -inkey server.key -in server.crt -certfile server.crt -passout pass:P@ssw0rd

COPY --from=publish /app .
ENTRYPOINT ["dotnet", "PrimeChecker.API.dll"]