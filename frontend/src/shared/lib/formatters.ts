import { format, parseISO } from 'date-fns';
import { ru } from 'date-fns/locale';

import { CURRENCY_SYMBOL } from '@/shared/config/constants';

const numberFormatter = new Intl.NumberFormat('ru-RU', { maximumFractionDigits: 0 });
const compactNumberFormatter = new Intl.NumberFormat('ru-RU', {
  notation: 'compact',
  maximumFractionDigits: 1,
});
const percentFormatter = new Intl.NumberFormat('ru-RU', {
  style: 'percent',
  minimumFractionDigits: 1,
  maximumFractionDigits: 1,
});

export function formatCurrency(value: number): string {
  return `${numberFormatter.format(Math.round(value))} ${CURRENCY_SYMBOL}`;
}

export function formatCompactCurrency(value: number): string {
  return `${compactNumberFormatter.format(value)} ${CURRENCY_SYMBOL}`;
}

export function formatNumber(value: number): string {
  return numberFormatter.format(value);
}

export function formatCompactNumber(value: number): string {
  return compactNumberFormatter.format(value);
}

/** Expects a 0..1 ratio, e.g. 0.234 -> "23.4%". */
export function formatMargin(ratio: number): string {
  return percentFormatter.format(ratio);
}

/** Expects an already-percent delta, e.g. 12.5 -> "+12.5%". */
export function formatChangePercent(percent: number): string {
  const sign = percent > 0 ? '+' : '';
  return `${sign}${percentFormatter.format(percent / 100)}`;
}

export function formatDate(iso: string, pattern = 'd MMM yyyy'): string {
  return format(parseISO(iso), pattern, { locale: ru });
}

export function formatDateTime(iso: string): string {
  return format(parseISO(iso), 'd MMM yyyy, HH:mm', { locale: ru });
}
