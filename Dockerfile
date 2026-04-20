# 1. Usamos el motor de .NET para compilar tu código
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos todo tu código a la nube
COPY . ./
# Descargamos las librerías (como Entity Framework)
RUN dotnet restore
# Compilamos el proyecto en modo "Release" (Optimizado)
RUN dotnet publish -c Release -o out

# 2. Usamos un motor más ligero solo para correr la API
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Le decimos a Render qué archivo encender (¡Asegúrate de que este sea el nombre exacto de tu proyecto!)
ENTRYPOINT ["dotnet", "CalculadoraNotasAPI.dll"]