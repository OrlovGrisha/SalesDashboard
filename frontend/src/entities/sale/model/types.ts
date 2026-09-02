export type SaleStatusLabel = 'Paid' | 'Cancelled' | 'Refunded';

export interface RecentSale {
  saleId: string;
  saleDate: string;
  managerName: string;
  customerName: string;
  customerCompany: string;
  statusLabel: SaleStatusLabel;
  productNames: string[];
  revenue: number;
  grossProfit: number;
}
