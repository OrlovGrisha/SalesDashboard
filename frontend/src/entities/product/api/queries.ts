import { useQuery } from '@tanstack/react-query';

import type { TopProduct } from '../model/types';

import { apiClient, toPeriodQuery, type PeriodParams } from '@/shared/api';

async function fetchTopProducts(period: PeriodParams, take: number): Promise<TopProduct[]> {
  const { data } = await apiClient.get<TopProduct[]>('/analytics/products/top', {
    params: { ...toPeriodQuery(period), take },
  });
  return data;
}

export function useTopProductsQuery(period: PeriodParams, take: number) {
  return useQuery({
    queryKey: ['analytics', 'products', 'top', period, take],
    queryFn: () => fetchTopProducts(period, take),
    placeholderData: (previousData) => previousData,
  });
}
