FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS builder
WORKDIR /source
COPY Patricia.ChatBot/Patricia.ChatBot.csproj Patricia.ChatBot/
RUN dotnet restore Patricia.ChatBot/Patricia.ChatBot.csproj
COPY . .
RUN dotnet publish Patricia.ChatBot/Patricia.ChatBot.csproj \
    -c Release -o /app --no-restore /p:UseAppHost=false
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=builder /app .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Patricia.ChatBot.dll"]