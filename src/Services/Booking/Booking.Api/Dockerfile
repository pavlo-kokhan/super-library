#Базовий runtime-образ .NET (без SDK) для фінального запуску сервісу. Тег AS base дає ім’я стадії.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#Запуск під некореневим користувачем
USER $APP_UID
#Робоча директорія в контейнері. Усі наступні команди виконуватимуться звідси.
WORKDIR /app
#Декларація внутрішніх портів, які слухає застосунок. Реально порти публікуються в docker-compose.yml.
EXPOSE 8080
EXPOSE 8081

#Окрема стадія збірки — SDK-образ з dotnet CLI, тут компілюємо код.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
#Параметр збірки з дефолтом Release (перевантажено як Development в compose.yml для візуалізації scalar).
ARG BUILD_CONFIGURATION=Release
#Робоча директорія стадії збірки.
WORKDIR /src
#Копіюємо лише csproj, щоб кешувався restore. Це прискорює повторні збірки.
COPY ["Services/Booking/Booking.Api/Booking.Api.csproj", "Services/Booking/Booking.Api/"]
#Відновлюємо NuGet-залежності (кешується окремим шаром).
RUN dotnet restore "Services/Booking/Booking.Api/Booking.Api.csproj"
#Дотягуємо увесь код (після кешованого restore).
COPY . .
#Переходимо в каталог проєкту для подальших команд.
WORKDIR "/src/Services/Booking/Booking.Api"
#Компіляція (перевірка, генеруються проміжні артефакти у /app/build).
RUN dotnet build "./Booking.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

#Нова стадія на основі результатів build для публікації.
FROM build AS publish
#Повторно оголошуємо ARG (область дії — в межах стадії).
ARG BUILD_CONFIGURATION=Release
#Публікуємо self-contained артефакти для runtime-образу. UseAppHost=false зменшує вагу (не створює платформний exe-хост).
RUN dotnet publish "./Booking.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

#Фінальна стадія: легкий runtime-образ.
FROM base AS final
#Робоча директорія у фінальному контейнері.
WORKDIR /app
#Копіюємо лише опубліковані артефакти з попередньої стадії (multi-stage оптимізація).
COPY --from=publish /app/publish .
#Точка входу: запуск ASP.NET Core сервісу.
ENTRYPOINT ["dotnet", "Booking.Api.dll"]
