FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore services/ProposalService/ProposalService.Api/ProposalService.Api.csproj
RUN dotnet publish services/ProposalService/ProposalService.Api/ProposalService.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProposalService.Api.dll"]
