import { AnimatePresence, motion } from 'framer-motion';
import { useEffect, useState } from 'react';

import { usePeriodStore } from '../model/periodStore';

import { CUSTOM_RANGE_DEBOUNCE_MS } from '@/shared/config/constants';
import { useDebouncedValue } from '@/shared/lib';

export function CustomRangePicker() {
  const preset = usePeriodStore((state) => state.preset);
  const customFrom = usePeriodStore((state) => state.customFrom);
  const customTo = usePeriodStore((state) => state.customTo);
  const setCustomRange = usePeriodStore((state) => state.setCustomRange);

  const [from, setFrom] = useState(customFrom ?? '');
  const [to, setTo] = useState(customTo ?? '');
  const debouncedFrom = useDebouncedValue(from, CUSTOM_RANGE_DEBOUNCE_MS);
  const debouncedTo = useDebouncedValue(to, CUSTOM_RANGE_DEBOUNCE_MS);

  useEffect(() => {
    if (debouncedFrom && debouncedTo && debouncedFrom <= debouncedTo) {
      setCustomRange(debouncedFrom, debouncedTo);
    }
  }, [debouncedFrom, debouncedTo, setCustomRange]);

  const isVisible = preset === 'Custom';

  return (
    <AnimatePresence initial={false}>
      {isVisible && (
        <motion.div
          initial={{ opacity: 0, width: 0 }}
          animate={{ opacity: 1, width: 'auto' }}
          exit={{ opacity: 0, width: 0 }}
          transition={{ duration: 0.2 }}
          className="flex items-center gap-2 overflow-hidden"
        >
          <input
            type="date"
            aria-label="Дата начала"
            value={from}
            max={to || undefined}
            onChange={(event) => setFrom(event.target.value)}
            className="rounded-lg border border-border bg-surface-card px-2.5 py-1.5 text-xs text-text-primary outline-none focus:ring-2 focus:ring-[var(--color-series-1)]"
          />
          <span className="text-xs text-text-muted">—</span>
          <input
            type="date"
            aria-label="Дата окончания"
            value={to}
            min={from || undefined}
            onChange={(event) => setTo(event.target.value)}
            className="rounded-lg border border-border bg-surface-card px-2.5 py-1.5 text-xs text-text-primary outline-none focus:ring-2 focus:ring-[var(--color-series-1)]"
          />
        </motion.div>
      )}
    </AnimatePresence>
  );
}
