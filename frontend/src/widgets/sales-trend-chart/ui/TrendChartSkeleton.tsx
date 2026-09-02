import { Skeleton } from '@/shared/ui';

export function TrendChartSkeleton() {
  return (
    <div className="flex h-72 items-end gap-2 px-1 pb-4">
      {Array.from({ length: 14 }).map((_, index) => (
        <Skeleton
          key={index}
          className="flex-1"
          style={{ height: `${30 + Math.abs(Math.sin(index)) * 60}%` }}
        />
      ))}
    </div>
  );
}
