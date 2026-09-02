# Sales Performance Dashboard — Backend

Тестового задания DJI-Market.ru: REST API для дашборда аналитики продаж менеджеров.

## Стек

- **.NET 8**, ASP.NET Core Web API
- **Entity Framework Core 8** — миграции, LINQ-агрегации
- **PostgreSQL**
- **Scalar** — интерактивная документация API (аналог Swagger UI)

- **React 19 + TypeScript**, **Vite**
- **TanStack Query** — запросы к API, кэширование, состояния loading/error
- **Zustand** — UI-состояние (выбранный период, режим рейтинга менеджеров)
- **Tailwind CSS v4** — стилизация, дизайн-токены через CSS-переменные
- **Recharts** — линейный график динамики и bar-chart категорий
- **Framer Motion** — микроанимации (появление карточек, layout-анимации, счётчики)
- **date-fns** — форматирование дат
- **Vitest + React Testing Library** — тесты

## Как запускать?

`docker compose up --build -d`

## Архитектура

### Backend

Backend построен по принципам Clean Architecture с элементами DDD, без CQRS.

Зависимости идут строго в одну сторону: `Domain` ← `Application` ← `Infrastructure`/`Api` ← `Host`. Domain не знает ни о чём снаружи; Application не знает про EF Core или ASP.NET Core.

### Frontend

Проект следует **Feature-Sliced Design**:

```
src/
├── app/        # провайдеры (React Query), глобальные стили
├── pages/      # DashboardPage — сборка виджетов в layout
├── widgets/    # самостоятельные блоки: kpi-summary, period-selector,
│               # sales-trend-chart, manager-ranking, category-breakdown,
│               # top-products, recent-sales-table
├── features/   # пользовательские сценарии: select-period, switch-ranking-mode
├── entities/   # бизнес-сущности: sale, manager, product, category, analytics
└── shared/     # api-клиент, UI-кит, форматтеры, конфиг

## Бизнес-правила

### Revenue, Gross Profit, Margin, Average Check

- **Revenue** — сумма по продажам со статусом `Paid` за период.
- **Gross Profit** = Revenue − Cost (себестоимость по тем же продажам).
- **Margin** = Gross Profit / Revenue (0, если Revenue = 0).
- **Average Check** = Revenue / количество Paid-продаж за период (0, если продаж не было).

### Трактовка статусов Cancelled и Refunded

Оба статуса **исключены** из Revenue/Gross Profit/Average Check — трактуются одинаково, как не генерирующие выручку. Это решение принято для простоты и консистентности: не нужно разбираться в промежуточных состояниях, метрики остаются однозначными по всей системе.

Отдельно от основных KPI считается `RefundedAmount` — сумма по Refunded-продажам, доступная для отдельной аналитики (доля возвратов), не смешивается с основным Revenue.

## Что не успел

Тесты к бекенду