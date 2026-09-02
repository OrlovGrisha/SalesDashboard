import { motion } from 'framer-motion';

import { ProductNamesCell } from './ProductNamesCell';
import { RecentSalesSkeleton } from './RecentSalesSkeleton';

import { StatusBadge, useRecentSalesQuery } from '@/entities/sale';
import { usePeriodParams } from '@/features/select-period';
import { DEFAULT_RECENT_SALES_TAKE } from '@/shared/config/constants';
import { formatCurrency, formatDateTime } from '@/shared/lib';
import {
  Card,
  CardBody,
  CardHeader,
  CardTitle,
  EmptyState,
  ErrorState,
  WidgetSurface,
} from '@/shared/ui';

const columnHeaderClass = 'px-3 py-2 text-left text-xs font-medium text-text-secondary';

export function RecentSalesTableWidget() {
  const period = usePeriodParams();
  const { data, isPending, isError, isFetching, refetch } = useRecentSalesQuery(
    period,
    DEFAULT_RECENT_SALES_TAKE,
  );

  const hasData = (data?.length ?? 0) > 0;

  return (
    <Card>
      <CardHeader>
        <CardTitle>Последние продажи</CardTitle>
      </CardHeader>
      <CardBody>
        {isPending ? (
          <RecentSalesSkeleton />
        ) : isError ? (
          <ErrorState message="Не удалось загрузить продажи" onRetry={() => refetch()} />
        ) : !hasData ? (
          <EmptyState />
        ) : (
          <WidgetSurface isRefetching={isFetching}>
            <div className="overflow-x-auto">
              <table className="w-full min-w-[900px] border-collapse">
                <thead>
                  <tr className="border-b border-border">
                    <th className={columnHeaderClass}>Дата</th>
                    <th className={columnHeaderClass}>Менеджер</th>
                    <th className={columnHeaderClass}>Клиент</th>
                    <th className={columnHeaderClass}>Товары</th>
                    <th className={columnHeaderClass}>Статус</th>
                    <th className="px-3 py-2 text-right text-xs font-medium text-text-secondary">
                      Сумма
                    </th>
                    <th className="px-3 py-2 text-right text-xs font-medium text-text-secondary">
                      Прибыль
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {data.map((sale, index) => (
                    <motion.tr
                      key={sale.saleId}
                      initial={{ opacity: 0 }}
                      animate={{ opacity: 1 }}
                      transition={{ duration: 0.2, delay: Math.min(index, 10) * 0.02 }}
                      className="border-b border-border/60 text-sm last:border-b-0 hover:bg-surface"
                    >
                      <td className="whitespace-nowrap px-3 py-2.5 tabular-nums text-text-secondary">
                        {formatDateTime(sale.saleDate)}
                      </td>
                      <td className="px-3 py-2.5 font-medium text-text-primary">
                        {sale.managerName}
                      </td>
                      <td className="px-3 py-2.5 text-text-primary">
                        <div>{sale.customerName}</div>
                        <div className="text-xs text-text-muted">{sale.customerCompany}</div>
                      </td>
                      <td className="max-w-56 px-3 py-2.5 text-text-secondary">
                        <ProductNamesCell names={sale.productNames} />
                      </td>
                      <td className="px-3 py-2.5">
                        <StatusBadge status={sale.statusLabel} />
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-primary">
                        {formatCurrency(sale.revenue)}
                      </td>
                      <td className="px-3 py-2.5 text-right tabular-nums text-text-secondary">
                        {formatCurrency(sale.grossProfit)}
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
