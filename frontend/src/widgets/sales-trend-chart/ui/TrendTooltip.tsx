import { formatDate } from '@/shared/lib';

interface TrendTooltipPayloadItem {
  name: string;
  value: number;
  color: string;
  formattedValue: string;
}

interface TrendTooltipProps {
  active?: boolean;
  label?: string;
  items?: TrendTooltipPayloadItem[];
}

export function TrendTooltip({ active, label, items }: TrendTooltipProps) {
  if (!active || !items || items.length === 0 || !label) return null;

  return (
    <div className="rounded-lg border border-border bg-surface-card px-3 py-2 shadow-card">
      <p className="text-xs font-medium text-text-secondary">{formatDate(label)}</p>
      <div className="mt-1.5 flex flex-col gap-1">
        {items.map((item) => (
          <div key={item.name} className="flex items-center gap-2 text-xs">
            <span className="size-2 rounded-full" style={{ backgroundColor: item.color }} />
            <span className="text-text-secondary">{item.name}</span>
            <span className="ml-auto font-medium tabular-nums text-text-primary">
              {item.formattedValue}
            </span>
          </div>
        ))}
      </div>
    </div>
  );
}
