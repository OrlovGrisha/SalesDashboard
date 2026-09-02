import { useQuery } from '@tanstack/react-query';

import type { RecentSale } from '../model/types';

import { apiClient, toPeriodQuery, type PeriodParams } from '@/shared/api';

async function fetchRecentSales(period: PeriodParams, take: number): Promise<RecentSale[]> {
  const { data } = await apiClient.get<RecentSale[]>('/sales/recent', {
    params: { ...toPeriodQuery(period), take },
  });
  return data;
}

export function useRecentSalesQuery(period: PeriodParams, take: number) {
  return useQuery({
    queryKey: ['sales', 'recent', period, take],
    queryFn: () => fetchRecentSales(period, take),
    placeholderData: (previousData) => previousData,
  });
}
