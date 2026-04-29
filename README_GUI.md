# MdInternals GUI

## Назначение
Windows GUI для запуска операций MdInternals: конвертация пакетов, задел под декомпиляцию ОП-кода и подключение к MSSQL 1C.

## Системные требования
- Windows 10/11 x64
- .NET 6 SDK (для сборки)

## Сборка
```bash
dotnet restore MdInternals.Gui/MdInternals.Gui.csproj
dotnet build MdInternals.Gui/MdInternals.Gui.csproj -c Release
```

## Запуск
```bash
dotnet run --project MdInternals.Gui/MdInternals.Gui.csproj -c Debug
```

## Публикация .exe
```bash
dotnet publish MdInternals.Gui/MdInternals.Gui.csproj \
  -c Release \
  -r win-x64 \
  -p:PublishSingleFile=true \
  -p:SelfContained=false \
  --output ./publish
```

## Вкладки
1. Конвертация форматов — загрузка `.cf/.cfu/.epf/.erf`, запуск экспорта, отмена, прогресс.
2. Декомпилятор ОП-кода — выбор файла и кодировки, просмотр результата, TODO в ядре.
3. Подключение к БД 1С — форма теста MSSQL и дерево объектов (без фальшивого успеха).
4. Настройки — язык, тема, кодировка, путь, уровень логов; сохраняются в `%AppData%\MdInternals\settings.json`.
5. О программе и логи — версия/лицензия/репозиторий, очистка и экспорт логов.

## Типовые сценарии
- Экспорт структуры пакета в текстовый summary (временный безопасный путь вместо полного XML).
- Проверка доступности функциональности декомпилятора/БД с понятными предупреждениями.

## Ограничения
- В публичном API ядра не найдена готовая функция полного XML export/rebuild: добавлен TODO и безопасное предупреждение.
- В публичном API ядра не найден прямой OP-code decompiler API.
- В окружении Linux GUI не запускается, поскольку проект `net6.0-windows`.

## Troubleshooting
- Ошибка `NETSDK1100`: собирать/публиковать проект на Windows с установленным Desktop targeting pack.
- Если `%AppData%` недоступен, проверьте права пользователя.

## Лицензия
GUI использует ту же лицензию GPL-3.0, что и проект.
Исходный репозиторий: https://github.com/sanchozu/MdInternals
