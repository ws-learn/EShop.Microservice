FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

USER app

# -- Build stage ---
FROM --platform=linux/amd64 mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/Services/Discount/Discount.Grpc/Discount.Grpc.csproj", "src/Services/Discount/Discount.Grpc/"]
RUN dotnet restore "src/Services/Discount/Discount.Grpc/Discount.Grpc.csproj"
COPY . .
WORKDIR "/src/src/Services/Discount/Discount.Grpc"
RUN dotnet build "Discount.Grpc.csproj" -c $configuration -o /app/build

# -- Publish stage ---
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Discount.Grpc.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# -- Final stage ---
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Discount.Grpc.dll"]
