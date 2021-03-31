FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY . .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime && echo 'Asia/Shanghai' >/etc/timezone
ENTRYPOINT ["dotnet", "Host.dll"]