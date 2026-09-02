import { motion } from 'framer-motion';

import { TopProductsSkeleton } from './TopProductsSkeleton';

import { useTopProductsQuery } from '@/entities/product';
import { usePeriodParams } from '@/features/select-period';
import { DEFAULT_TOP_PRODUCTS_TAKE } from '@/shared/config/constants';
import { formatCurrency, formatNumber } from '@/shared/lib';
import {
  Card,
  CardBody,
  CardHeader,
  CardTitle,
  EmptyState,
  ErrorState,
  WidgetSurface,
} from '@/shared/ui';

const listVariants = {
  hidden: {},
  visible: { transition: { staggerChildren: 0.04 } },
};
const rowVariants = {
  hidden: { opacity: 0, y: 8 },
  visible: { opacity: 1, y: 0 },
};

export function TopProductsWidget() {
  const period = usePeriodParams();
  const { data, isPending, isError, isFetching, refetch } = useTopProductsQuery(
    period,
    DEFAULT_TOP_PRODUCTS_TAKE,
  );

  const hasData = (data?.length ?? 0) > 0;
  const maxRevenue = Math.max(1, ...(data?.map((product) => product.revenue) ?? [1]));

  return (
    <Card>
      <CardHeader>
        <CardTitle>Топ товаров</CardTitle>
      </CardHeader>
      <CardBody>
        {isPending ? (
          <TopProductsSkeleton />
        ) : isError ? (
          <ErrorState message="Не удалось загрузить товары" onRetry={() => refetch()} />
        ) : !hasData ? (
          <EmptyState />
        ) : (
          <WidgetSurface isRefetching={isFetching}>
            <motion.ol
              className="flex flex-col"
              variants={listVariants}
              initial="hidden"
              animate="visible"
            >
              {data.map((product, index) => (
                <motion.li
                  key={product.productId}
                  variants={rowVariants}
                  transition={{ duration: 0.2 }}
                  className="relative flex items-center gap-3 rounded-lg px-2 py-2.5 hover:bg-surface"
                >
                  <div
                    className="absolute inset-y-0 left-0 rounded-lg bg-[color-mix(in_oklab,var(--color-series-1)_10%,transparent)]"
                    style={{ width: `${(product.revenue / maxRevenue) * 100}%` }}
                  />
                  <span className="relative flex size-6 shrink-0 items-center justify-center rounded-full bg-surface text-xs font-medium text-text-secondary">
                    {index + 1}
                  </span>
                  <div className="relative min-w-0 flex-1">
                    <p className="truncate text-sm font-medium text-text-primary">
                      {product.productName}
                    </p>
                    <p className="text-xs text-text-muted">
                      {formatNumber(product.quantitySold)} шт.
                    </p>
                  </div>
                  <div className="relative shrink-0 text-right">
                    <p className="text-sm font-medium tabular-nums text-text-primary">
                      {formatCurrency(product.revenue)}
                    </p>
                    <p className="text-xs tabular-nums text-text-muted">
                      {formatCurrency(product.grossProfit)}
                    </p>
                  </div>
                </motion.li>
              ))}
            </motion.ol>
          </WidgetSurface>
        )}
      </CardBody>
    </Card>
  );
}
