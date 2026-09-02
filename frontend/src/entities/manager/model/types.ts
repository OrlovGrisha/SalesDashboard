export type RankingMode = 'GrossProfit' | 'AverageCheck';

export interface ManagerRankingItem {
  rank: number;
  managerId: string;
  managerName: string;
  salesCount: number;
  revenue: number;
  grossProfit: number;
  averageCheck: number;
  margin: number;
  changePercent: number;
}
