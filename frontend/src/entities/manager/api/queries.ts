import { useQuery } from '@tanstack/react-query';

import type { ManagerRankingItem, RankingMode } from '../model/types';

import { apiClient, toPeriodQuery, type PeriodParams } from '@/shared/api';

async function fetchManagerRanking(
  period: PeriodParams,
  mode: RankingMode,
): Promise<ManagerRankingItem[]> {
  const { data } = await apiClient.get<ManagerRankingItem[]>('/managers/ranking', {
    params: { ...toPeriodQuery(period), mode },
  });
  return data;
}

export function useManagerRankingQuery(period: PeriodParams, mode: RankingMode) {
  return useQuery({
    queryKey: ['managers', 'ranking', period, mode],
    queryFn: () => fetchManagerRanking(period, mode),
    placeholderData: (previousData) => previousData,
  });
}
