# Intro

此範例是為了練習透過 docker 佈署，並於更版當下，利用 shared-cookie 避免網站的使用者被登出系統

> 採用 dotnetCore 2.1

## dockerfile 
```dockerfile
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /out .
ENTRYPOINT ["dotnet", "shared-cookie-redis.dll"]
```
## 建立 docker image
```
docker build -t demo .
```

## 建立 container
```
docker run -d --rm --name=mysite -p 7000:80 demo
```

