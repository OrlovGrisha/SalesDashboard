import type { HTMLAttributes } from 'react';

import { cn } from '@/shared/lib';

export function Skeleton({ className, ...props }: HTMLAttributes<HTMLDivElement>) {
  return (
    <div
      role="status"
      aria-label="Загрузка"
      className={cn('animate-pulse rounded-md bg-skeleton', className)}
      {...props}
    />
  );
}
