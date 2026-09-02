import { endOfDay, startOfDay } from 'date-fns';
import { useMemo } from 'react';

import { usePeriodStore } from './periodStore';

import type { PeriodParams } from '@/shared/api';

/** Resolves the current selection into the query-param shape all analytics endpoints expect. */
export function usePeriodParams(): PeriodParams {
  const preset = usePeriodStore((state) => state.preset);
  const customFrom = usePeriodStore((state) => state.customFrom);
  const customTo = usePeriodStore((state) => state.customTo);

  return useMemo<PeriodParams>(() => {
    if (preset === 'Custom' && customFrom && customTo) {
      return {
        preset,
        from: startOfDay(new Date(customFrom)).toISOString(),
        to: endOfDay(new Date(customTo)).toISOString(),
      };
    }
    return { preset };
  }, [preset, customFrom, customTo]);
}
