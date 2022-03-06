#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BlazorServerLoginLogoutDemo.Web/BlazorServerLoginLogoutDemo.Web.csproj", "src/BlazorServerLoginLogoutDemo.Web/"]
RUN dotnet restore "src/BlazorServerLoginLogoutDemo.Web/BlazorServerLoginLogoutDemo.Web.csproj"
COPY . .
WORKDIR "/src/src/BlazorServerLoginLogoutDemo.Web"
RUN dotnet build "BlazorServerLoginLogoutDemo.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorServerLoginLogoutDemo.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServerLoginLogoutDemo.Web.dll"]