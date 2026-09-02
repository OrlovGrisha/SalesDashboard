import { useQuery } from '@tanstack/react-query';

import type { CategoryBreakdownItem } from '../model/types';

import { apiClient, toPeriodQuery, type PeriodParams } from '@/shared/api';

async function fetchCategories(period: PeriodParams): Promise<CategoryBreakdownItem[]> {
  const { data } = await apiClient.get<CategoryBreakdownItem[]>('/analytics/categories', {
    params: toPeriodQuery(period),
  });
  return data;
}

export function useCategoriesQuery(period: PeriodParams) {
  return useQuery({
    queryKey: ['analytics', 'categories', period],
    queryFn: () => fetchCategories(period),
    placeholderData: (previousData) => previousData,
  });
}
