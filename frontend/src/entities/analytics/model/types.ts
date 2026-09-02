export type TrendGranularity = 'Day' | 'Week' | 'Month';

export interface KpiComparison {
  revenueChangePercent: number;
  grossProfitChangePercent: number;
  salesCountChangePercent: number;
  averageCheckChangePercent: number;
}

export interface KpiData {
  revenue: number;
  grossProfit: number;
  margin: number;
  salesCount: number;
  averageCheck: number;
  topManagerName: string | null;
  comparison: KpiComparison;
}

export interface TrendPoint {
  periodStart: string;
  revenue: number;
  grossProfit: number;
  salesCount: number;
}
