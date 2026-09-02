import type { HTMLAttributes } from 'react';

import { cn } from '@/shared/lib';

export type BadgeTone = 'success' | 'neutral' | 'warning' | 'info';

const toneClasses: Record<BadgeTone, string> = {
  success:
    'bg-[color-mix(in_oklab,var(--color-good)_14%,var(--color-surface-card))] text-[var(--color-good-text)]',
  warning:
    'bg-[color-mix(in_oklab,var(--color-serious)_16%,var(--color-surface-card))] text-[var(--color-serious-text)]',
  info: 'bg-[color-mix(in_oklab,var(--color-series-1)_14%,var(--color-surface-card))] text-[var(--color-series-1-text)]',
  neutral:
    'bg-[color-mix(in_oklab,var(--color-text-muted)_16%,var(--color-surface-card))] text-text-secondary',
};

interface BadgeProps extends HTMLAttributes<HTMLSpanElement> {
  tone?: BadgeTone;
}

export function Badge({ tone = 'neutral', className, ...props }: BadgeProps) {
  return (
    <span
      className={cn(
        'inline-flex items-center gap-1 rounded-full px-2.5 py-0.5 text-xs font-medium',
        toneClasses[tone],
        className,
      )}
      {...props}
    />
  );
}
