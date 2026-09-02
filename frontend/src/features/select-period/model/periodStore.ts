import { create } from 'zustand';

import type { PeriodPreset } from '@/shared/api';

interface PeriodState {
  preset: PeriodPreset;
  /** yyyy-MM-dd from a native date input, only meaningful when preset === 'Custom'. */
  customFrom: string | null;
  customTo: string | null;
  setPreset: (preset: PeriodPreset) => void;
  setCustomRange: (from: string | null, to: string | null) => void;
}

export const usePeriodStore = create<PeriodState>((set) => ({
  preset: 'Last30Days',
  customFrom: null,
  customTo: null,
  setPreset: (preset) =>
    set({ preset, ...(preset === 'Custom' ? {} : { customFrom: null, customTo: null }) }),
  setCustomRange: (customFrom, customTo) => set({ preset: 'Custom', customFrom, customTo }),
}));
