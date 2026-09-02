import { useState } from 'react';
import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts';

import { TrendChartSkeleton } from './TrendChartSkeleton';
import { TrendTooltip } from './TrendTooltip';

import type { TrendGranularity } from '@/entities/analytics';
import { useTrendQuery } from '@/entities/analytics';
import { usePeriodParams } from '@/features/select-period';
import {
  formatCompactCurrency,
  formatCompactNumber,
  formatCurrency,
  formatDate,
  formatNumber,
} from '@/shared/lib';
import {
  Card,
  CardBody,
  CardHeader,
  CardTitle,
  EmptyState,
  ErrorState,
  SegmentedControl,
  WidgetSurface,
  type SegmentedOption,
} from '@/shared/ui';

type MetricView = 'money' | 'count';

const granularityOptions: SegmentedOption<TrendGranularity>[] = [
  { value: 'Day', label: 'День' },
  { value: 'Week', label: 'Неделя' },
  { value: 'Month', label: 'Месяц' },
];

const metricOptions: SegmentedOption<MetricView>[] = [
  { value: 'money', label: 'Выручка и прибыль' },
  { value: 'count', label: 'Кол-во продаж' },
];

function tickFormatterFor(granularity: TrendGranularity) {
  const pattern = granularity === 'Month' ? 'LLL yyyy' : 'd MMM';
  return (value: string) => formatDate(value, pattern);
}

export function SalesTrendChartWidget() {
  const period = usePeriodParams();
  const [granularity, setGranularity] = useState<TrendGranularity>('Day');
  const [metric, setMetric] = useState<MetricView>('money');

  const { data, isPending, isError, isFetching, refetch } = useTrendQuery(period, granularity);

  const hasData = (data?.length ?? 0) > 0;

  return (
    <Card>
      <CardHeader>
        <CardTitle>Динамика продаж</CardTitle>
        <div className="flex items-center gap-2">
          <SegmentedControl
            layoutId="trend-metric"
            options={metricOptions}
            value={metric}
            onChange={setMetric}
          />
          <SegmentedControl
            layoutId="trend-granularity"
            options={granularityOptions}
            value={granularity}
            onChange={setGranularity}
          />
        </div>
      </CardHeader>
      <CardBody>
        {isPending ? (
          <TrendChartSkeleton />
        ) : isError ? (
          <ErrorState message="Не удалось загрузить динамику продаж" onRetry={() => refetch()} />
        ) : !hasData ? (
          <EmptyState />
        ) : (
          <WidgetSurface isRefetching={isFetching}>
            <div className="h-72">
              <ResponsiveContainer width="100%" height="100%">
                <LineChart data={data} margin={{ top: 8, right: 8, left: 0, bottom: 0 }}>
                  <CartesianGrid stroke="var(--color-grid)" vertical={false} />
                  <XAxis
                    dataKey="periodStart"
                    tickFormatter={tickFormatterFor(granularity)}
                    tick={{ fill: 'var(--color-text-muted)', fontSize: 12 }}
                    axisLine={{ stroke: 'var(--color-axis)' }}
                    tickLine={false}
                  />
                  <YAxis
                    tickFormatter={metric === 'money' ? formatCompactCurrency : formatCompactNumber}
                    tick={{ fill: 'var(--color-text-muted)', fontSize: 12 }}
                    axisLine={false}
                    tickLine={false}
                    width={56}
                  />
                  <Tooltip
                    cursor={{ stroke: 'var(--color-axis)', strokeWidth: 1 }}
                    content={({ active, label, payload }) => (
                      <TrendTooltip
                        active={active}
                        label={label as string}
                        items={payload?.map((entry) => ({
                          name: entry.name as string,
                          value: entry.value as number,
                          color: entry.color as string,
                          formattedValue:
                            metric === 'money'
                              ? formatCurrency(entry.value as number)
                              : formatNumber(entry.value as number),
                        }))}
                      />
                    )}
                  />
                  {metric === 'money' && (
                    <Legend wrapperStyle={{ fontSize: 12, color: 'var(--color-text-secondary)' }} />
                  )}
                  {metric === 'money' ? (
                    <>
                      <Line
                        type="monotone"
                        dataKey="revenue"
                        name="Выручка"
                        stroke="var(--color-series-1)"
                        strokeWidth={2}
                        dot={false}
                        activeDot={{ r: 4, strokeWidth: 2, stroke: 'var(--color-surface-card)' }}
                        isAnimationActive
                        animationDuration={250}
                      />
                      <Line
                        type="monotone"
                        dataKey="grossProfit"
                        name="Валовая прибыль"
                        stroke="var(--color-series-2)"
                        strokeWidth={2}
                        dot={false}
                        activeDot={{ r: 4, strokeWidth: 2, stroke: 'var(--color-surface-card)' }}
                        isAnimationActive
                        animationDuration={250}
                      />
                    </>
                  ) : (
                    <Line
                      type="monotone"
                      dataKey="salesCount"
                      name="Кол-во продаж"
                      stroke="var(--color-series-1)"
                      strokeWidth={2}
                      dot={false}
                      activeDot={{ r: 4, strokeWidth: 2, stroke: 'var(--color-surface-card)' }}
                      isAnimationActive
                      animationDuration={250}
                    />
                  )}
                </LineChart>
              </ResponsiveContainer>
            </div>
          </WidgetSurface>
        )}
      </CardBody>
    </Card>
  );
}
