import { Card, CardBody, Skeleton } from '@/shared/ui';

export function KpiSummarySkeleton() {
  return (
    <div className="grid grid-cols-6 gap-4">
      {Array.from({ length: 6 }).map((_, index) => (
        <Card key={index}>
          <CardBody>
            <Skeleton className="h-3 w-20" />
            <Skeleton className="mt-2.5 h-7 w-24" />
            <Skeleton className="mt-2.5 h-4 w-12" />
          </CardBody>
        </Card>
      ))}
    </div>
  );
}
