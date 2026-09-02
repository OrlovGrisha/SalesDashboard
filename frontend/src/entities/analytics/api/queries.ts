import { useQuery } from '@tanstack/react-query';

import type { KpiData, TrendGranularity, TrendPoint } from '../model/types';

import { apiClient, toPeriodQuery, type PeriodParams } from '@/shared/api';

async function fetchKpi(period: PeriodParams): Promise<KpiData> {
  const { data } = await apiClient.get<KpiData>('/analytics/kpi', {
    params: toPeriodQuery(period),
  });
  return data;
}

export function useKpiQuery(period: PeriodParams) {
  return useQuery({
    queryKey: ['analytics', 'kpi', period],
    queryFn: () => fetchKpi(period),
    placeholderData: (previousData) => previousData,
  });
}

async function fetchTrend(
  period: PeriodParams,
  granularity: TrendGranularity,
): Promise<TrendPoint[]> {
  const { data } = await apiClient.get<TrendPoint[]>('/analytics/trend', {
    params: { ...toPeriodQuery(period), granularity },
  });
  return data;
}

export function useTrendQuery(period: PeriodParams, granularity: TrendGranularity) {
  return useQuery({
    queryKey: ['analytics', 'trend', period, granularity],
    queryFn: () => fetchTrend(period, granularity),
    placeholderData: (previousData) => previousData,
  });
}
