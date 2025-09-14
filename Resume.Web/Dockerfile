# مرحله 1: ساخت
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# کپی فایل‌های csproj با نام واقعی پوشه‌ها
COPY ["Resume.Web/Resume.Web.csproj", "Resume.Web/"]
COPY ["Resume.Application/Resume.Application.csproj", "Resume.Application/"]
COPY ["Resume.Domain/Resume.Domain.csproj", "Resume.Domain/"]
COPY ["Resume.Infra.Data/Resume.Infra.Data.csproj", "Resume.Infra.Data/"]
COPY ["Resume.Infra.IoC/Resume.Infra.IoC.csproj", "Resume.Infra.IoC/"]

# رستور پکیج‌ها
RUN dotnet restore "Resume.Web/Resume.Web.csproj"

# کپی کل سورس
COPY . .

# بیلد
WORKDIR /src/Resume.Web
RUN dotnet build "Resume.Web.csproj" -c Release -o /app/build

# مرحله 2: پابلیش
FROM build AS publish
RUN dotnet publish "Resume.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# مرحله 3: ران‌تایم
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Resume.Web.dll"]
