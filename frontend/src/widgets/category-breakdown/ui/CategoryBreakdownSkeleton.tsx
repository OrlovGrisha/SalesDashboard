import { Skeleton } from '@/shared/ui';

export function CategoryBreakdownSkeleton() {
  return (
    <div className="flex flex-col gap-3">
      {Array.from({ length: 5 }).map((_, index) => (
        <div key={index} className="flex items-center gap-3">
          <Skeleton className="h-3 w-20" />
          <Skeleton className="h-5 flex-1" style={{ maxWidth: `${80 - index * 12}%` }} />
        </div>
      ))}
    </div>
  );
}
