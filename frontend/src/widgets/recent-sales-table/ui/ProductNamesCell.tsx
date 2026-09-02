export function ProductNamesCell({ names }: { names: string[] }) {
  if (names.length === 0) return <span className="text-text-muted">—</span>;

  const [first, ...rest] = names;

  return (
    <span className="truncate" title={names.join(', ')}>
      {first}
      {rest.length > 0 && <span className="ml-1 text-text-muted">+{rest.length}</span>}
    </span>
  );
}
