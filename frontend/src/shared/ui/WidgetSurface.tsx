import { type ReactNode } from 'react';

import { cn } from '@/shared/lib';

interface WidgetSurfaceProps {
  /** Dims + prevents interaction while a background refetch is in flight, without swapping content for a skeleton. */
  isRefetching?: boolean;
  children: ReactNode;
  className?: string;
}

/** Wraps a widget's data view so period changes show a soft dim instead of a full skeleton replace. */
export function WidgetSurface({ isRefetching, children, className }: WidgetSurfaceProps) {
  return (
    <div
      aria-busy={isRefetching}
      className={cn(
        'transition-opacity duration-200',
        isRefetching && 'pointer-events-none animate-pulse-soft opacity-60',
        className,
      )}
    >
      {children}
    </div>
  );
}
