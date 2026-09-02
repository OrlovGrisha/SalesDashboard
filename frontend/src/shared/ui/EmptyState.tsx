interface EmptyStateProps {
  message?: string;
  compact?: boolean;
}

export function EmptyState({
  message = 'Нет данных за выбранный период',
  compact,
}: EmptyStateProps) {
  return (
    <div
      className={`flex flex-col items-center justify-center gap-2 text-center ${compact ? 'py-6' : 'py-12'}`}
    >
      <div className="flex size-9 items-center justify-center rounded-full bg-surface">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          strokeWidth={2}
          strokeLinecap="round"
          strokeLinejoin="round"
          className="size-4.5 text-text-muted"
        >
          <rect x="3" y="3" width="18" height="18" rx="2" />
          <path d="M8 12h8" />
        </svg>
      </div>
      <p className="max-w-xs text-sm text-text-muted">{message}</p>
    </div>
  );
}
