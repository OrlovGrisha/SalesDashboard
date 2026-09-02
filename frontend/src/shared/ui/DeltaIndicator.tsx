import { formatChangePercent } from '@/shared/lib';

interface DeltaIndicatorProps {
  percent: number;
  className?: string;
}

export function DeltaIndicator({ percent, className }: DeltaIndicatorProps) {
  const isFlat = percent === 0;
  const isPositive = percent > 0;

  const colorClass = isFlat ? 'text-text-muted' : isPositive ? 'text-good-text' : 'text-bad-text';

  return (
    <span
      className={`inline-flex items-center gap-1 text-xs font-medium tabular-nums ${colorClass} ${className ?? ''}`}
    >
      {!isFlat && (
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          strokeWidth={2.5}
          strokeLinecap="round"
          strokeLinejoin="round"
          className={`size-3 ${isPositive ? '' : 'rotate-180'}`}
        >
          <path d="M12 19V5" />
          <path d="m5 12 7-7 7 7" />
        </svg>
      )}
      {formatChangePercent(percent)}
    </span>
  );
}
