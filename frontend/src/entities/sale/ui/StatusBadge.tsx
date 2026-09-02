import type { SaleStatusLabel } from '../model/types';

import { Badge, type BadgeTone } from '@/shared/ui';

const toneByStatus: Record<SaleStatusLabel, BadgeTone> = {
  Paid: 'success',
  Cancelled: 'neutral',
  Refunded: 'warning',
};

const labelByStatus: Record<SaleStatusLabel, string> = {
  Paid: 'Оплачено',
  Cancelled: 'Отменено',
  Refunded: 'Возврат',
};

export function StatusBadge({ status }: { status: SaleStatusLabel }) {
  return <Badge tone={toneByStatus[status]}>{labelByStatus[status]}</Badge>;
}
