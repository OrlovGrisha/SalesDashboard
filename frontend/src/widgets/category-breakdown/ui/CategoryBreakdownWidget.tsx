import {
  Bar,
  BarChart,
  CartesianGrid,
  Legend,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';

import { CategoryBreakdownSkeleton } from './CategoryBreakdownSkeleton';
import { CategoryTooltip } from './CategoryTooltip';

import { useCategoriesQuery } from '@/entities/category';
import { usePeriodParams } from '@/features/select-period';
import { formatCompactCurrency, formatCurrency } from '@/shared/lib';
import {
  Card,
  CardBody,
  CardHeader,
  CardTitle,
  EmptyState,
  ErrorState,
  WidgetSurface,
} from '@/shared/ui';

export function CategoryBreakdownWidget() {
  const period = usePeriodParams();
  const { data, isPending, isError, isFetching, refetch } = useCategoriesQuery(period);

  const hasData = (data?.length ?? 0) > 0;
  const chartHeight = Math.max((data?.length ?? 0) * 44, 160);

  return (
    <Card>
      <CardHeader>
        <CardTitle>Продажи по категориям</CardTitle>
      </CardHeader>
      <CardBody>
        {isPending ? (
          <CategoryBreakdownSkeleton />
        ) : isError ? (
          <ErrorState message="Не удалось загрузить категории" onRetry={() => refetch()} />
        ) : !hasData ? (
          <EmptyState />
        ) : (
          <WidgetSurface isRefetching={isFetching}>
            <div style={{ height: chartHeight }}>
              <ResponsiveContainer width="100%" height="100%">
                <BarChart
                  data={data}
                  layout="vertical"
                  margin={{ top: 0, right: 16, left: 0, bottom: 0 }}
                  barCategoryGap={12}
                  barGap={2}
                >
                  <CartesianGrid stroke="var(--color-grid)" horizontal={false} />
                  <XAxis
                    type="number"
                    tickFormatter={formatCompactCurrency}
                    tick={{ fill: 'var(--color-text-muted)', fontSize: 12 }}
                    axisLine={{ stroke: 'var(--color-axis)' }}
                    tickLine={false}
                  />
                  <YAxis
                    type="category"
                    dataKey="categoryName"
                    width={140}
                    tick={{ fill: 'var(--color-text-secondary)', fontSize: 12 }}
                    axisLine={false}
                    tickLine={false}
                  />
                  <Tooltip
                    cursor={{ fill: 'var(--color-surface)' }}
                    content={({ active, label, payload }) => (
                      <CategoryTooltip
                        active={active}
                        label={label as string}
                        items={payload?.map((entry) => ({
                          name: entry.name as string,
                          color: entry.color as string,
                          formattedValue: formatCurrency(entry.value as number),
                        }))}
                      />
                    )}
                  />
                  <Legend wrapperStyle={{ fontSize: 12, color: 'var(--color-text-secondary)' }} />
                  <Bar
                    dataKey="revenue"
                    name="Выручка"
                    fill="var(--color-series-1)"
                    radius={[0, 4, 4, 0]}
                    barSize={16}
                    isAnimationActive
                    animationDuration={250}
                  />
                  <Bar
                    dataKey="grossProfit"
                    name="Валовая прибыль"
                    fill="var(--color-series-2)"
                    radius={[0, 4, 4, 0]}
                    barSize={16}
                    isAnimationActive
                    animationDuration={250}
                  />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </WidgetSurface>
        )}
      </CardBody>
    </Card>
  );
}
