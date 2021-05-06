FROM mcr.microsoft.com/dotnet/sdk:5.0 as svcbuild
WORKDIR /src
copy . .
RUN dotnet build -c Release Oxygen-Dapr.EshopSample.sln
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as goodsservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/GoodsService/Host/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "Host.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as accountservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/AccountService/Host/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "Host.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as publicservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/PublicService/Host/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "Host.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as tradeservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/TradeService/Host/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "Host.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as imageservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/ImageService/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "ImageService.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as jobservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/JobService/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "JobService.dll"]
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as oauthservice
WORKDIR /app
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
COPY --from=svcbuild /src/Services/OauthService/bin/Release/net5.0 /app
ENTRYPOINT ["dotnet", "OauthService.dll"]



