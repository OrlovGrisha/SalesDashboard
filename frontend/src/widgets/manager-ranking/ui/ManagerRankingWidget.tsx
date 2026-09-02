import { motion } from 'framer-motion';

import { ManagerRankingSkeleton } from './ManagerRankingSkeleton';

import { useManagerRankingQuery } from '@/entities/manager';
import { usePeriodParams } from '@/features/select-period';
import { RankingModeToggle, useRankingModeStore } from '@/features/switch-ranking-mode';
import { formatCurrency, formatMargin, formatNumber } from '@/shared/lib';
import {
  Card,
  CardBody,
  CardHeader,
  CardTitle,
  DeltaIndicator,
  EmptyState,
  ErrorState,
  WidgetSurface,
} from '@/shared/ui';

const columnHeaderClass = 'px-3 py-2 text-right text-xs font-medium text-text-secondary';

export function ManagerRankingWidget() {
  const period = usePeriodParams();
  const mode = useRankingModeStore((state) => state.mode);
  const { data, isPending, isError, isFetching, refetch } = useManagerRankingQuery(period, mode);

  const hasData = (data?.length ?? 0) > 0;

  return (
    <Card>
      <CardHeader>
        <CardTitle>Рейтинг менеджеров</CardTitle>
        <RankingModeToggle />
      </CardHeader>
      <CardBody>
        {isPending ? (
          <ManagerRankingSkeleton />
        ) : isError ? (
          <ErrorState message="Не удалось загрузить рейтинг" onRetry={() => refetch()} />
        ) : !hasData ? (
          <EmptyState />
        ) : (
          <WidgetSurface isRefetching={isFetching}>
            <div className="overflow-x-auto">
              <table className="w-full border-collapse">
                <thead>
                  <tr className="border-b border-border">
                    <th className="px-3 py-2 text-left text-xs font-medium text-text-secondary">
                      #
                    </th>
                    <th className="px-3 py-2 text-left text-xs font-medium text-text-secondary">
                      Менеджер
                    </th>
                    <th className={columnHeaderClass}>Продажи</th>
                    <th className={columnHeaderClass}>Выручка</th>
                    <th className={columnHeaderClass}>Прибыль</th>
                    <th className={columnHeaderClass}>Ср. чек</th>
                    <th className={columnHeaderClass}>Маржа</th>
                    <th className={columnHeaderClass}>Δ</th>
                  </tr>
                </thead>
                <tbody>
                  {data.map((manager) => (
                    <motion.tr
                      key={manager.managerId}
                      layout
                      transition={{ duration: 0.25, ease: 'easeInOut' }}
                      className="border-b border-border/60 text-sm last:border-b-0 hover:bg-surface"
                    >
                      <td className="px-3 py-2.5 tabular-nums text-text-muted">{manager.rank}</td>
                      <td className="px-3 py-2.5 font-medium text-text-primary">
                        {manager.managerName}
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-secondary">
                        {formatNumber(manager.salesCount)}
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-primary">
                        {formatCurrency(manager.revenue)}
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-primary">
                        {formatCurrency(manager.grossProfit)}
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-secondary">
                        {formatCurrency(manager.averageCheck)}
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-secondary">
                        {formatMargin(manager.margin)}
                      </td>
                      <td className="px-3 py-2.5 text-right">
                        <DeltaIndicator percent={manager.changePercent} className="justify-end" />
                      </td>
                    </motion.tr>
                  ))}
                </tbody>
              </table>
            </div>
          </WidgetSurface>
        )}
      </CardBody>
    </Card>
  );
}
