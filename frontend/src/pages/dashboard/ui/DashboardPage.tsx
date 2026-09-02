import { CategoryBreakdownWidget } from '@/widgets/category-breakdown';
import { KpiSummaryWidget } from '@/widgets/kpi-summary';
import { ManagerRankingWidget } from '@/widgets/manager-ranking';
import { PeriodSelectorWidget } from '@/widgets/period-selector';
import { RecentSalesTableWidget } from '@/widgets/recent-sales-table';
import { SalesTrendChartWidget } from '@/widgets/sales-trend-chart';
import { TopProductsWidget } from '@/widgets/top-products';

export function DashboardPage() {
  return (
    <div className="mx-auto flex max-w-[1440px] flex-col gap-5 px-6 py-6">
      <header className="flex items-center justify-between">
        <div>
          <h1 className="text-lg font-semibold text-text-primary">Sales Performance Dashboard</h1>
          <p className="text-sm text-text-muted">Аналитика продаж менеджеров</p>
        </div>
        <PeriodSelectorWidget />
      </header>

      <KpiSummaryWidget />

      <div className="grid grid-cols-3 gap-5">
        <div className="col-span-2">
          <SalesTrendChartWidget />
        </div>
        <CategoryBreakdownWidget />
      </div>

      <div className="grid grid-cols-3 gap-5">
        <div className="col-span-2">
          <ManagerRankingWidget />
        </div>
        <TopProductsWidget />
      </div>

      <RecentSalesTableWidget />
    </div>
  );
}
