FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 58182
EXPOSE 44387

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["JasonSnake/JasonSnake.csproj", "JasonSnake/"]
RUN dotnet restore "JasonSnake/JasonSnake.csproj"
COPY . .
WORKDIR "/src/JasonSnake"
RUN dotnet build "JasonSnake.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "JasonSnake.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JasonSnake.dll"]