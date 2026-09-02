export type PeriodPreset =
  'Today' | 'Last7Days' | 'Last30Days' | 'ThisMonth' | 'LastMonth' | 'Custom';

export interface PeriodParams {
  preset: PeriodPreset;
  from?: string;
  to?: string;
}

/** Query params object suitable for axios `params` — stable shape for React Query cache keys. */
export function toPeriodQuery(period: PeriodParams): Record<string, string> {
  const query: Record<string, string> = { preset: period.preset };
  if (period.preset === 'Custom') {
    if (period.from) query.from = period.from;
    if (period.to) query.to = period.to;
  }
  return query;
}
