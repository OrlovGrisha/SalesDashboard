import { motion } from 'framer-motion';

import { KpiCard, TopManagerCard } from './KpiCard';
import { KpiSummarySkeleton } from './KpiSummarySkeleton';

import { useKpiQuery } from '@/entities/analytics';
import { usePeriodParams } from '@/features/select-period';
import { formatCurrency, formatMargin, formatNumber } from '@/shared/lib';
import { Card, CardBody, EmptyState, ErrorState, WidgetSurface } from '@/shared/ui';

const gridVariants = {
  hidden: {},
  visible: { transition: { staggerChildren: 0.05 } },
};

export function KpiSummaryWidget() {
  const period = usePeriodParams();
  const { data, isPending, isError, isFetching, refetch } = useKpiQuery(period);

  if (isPending) {
    return <KpiSummarySkeleton />;
  }

  if (isError) {
    return (
      <Card>
        <CardBody>
          <ErrorState message="Не удалось загрузить KPI" onRetry={() => refetch()} />
        </CardBody>
      </Card>
    );
  }

  if (data.salesCount === 0) {
    return (
      <Card>
        <CardBody>
          <EmptyState />
        </CardBody>
      </Card>
    );
  }

  return (
    <WidgetSurface isRefetching={isFetching}>
      <motion.div
        className="grid grid-cols-6 gap-4"
        variants={gridVariants}
        initial="hidden"
        animate="visible"
      >
        <KpiCard
          label="Выручка"
          value={data.revenue}
          format={formatCurrency}
          delta={data.comparison.revenueChangePercent}
        />
        <KpiCard
          label="Валовая прибыль"
          value={data.grossProfit}
          format={formatCurrency}
          delta={data.comparison.grossProfitChangePercent}
        />
        <KpiCard label="Маржа" value={data.margin} format={formatMargin} />
        <KpiCard
          label="Кол-во продаж"
          value={data.salesCount}
          format={formatNumber}
          delta={data.comparison.salesCountChangePercent}
        />
        <KpiCard
          label="Средний чек"
          value={data.averageCheck}
          format={formatCurrency}
          delta={data.comparison.averageCheckChangePercent}
        />
        <TopManagerCard name={data.topManagerName} />
      </motion.div>
    </WidgetSurface>
  );
}
