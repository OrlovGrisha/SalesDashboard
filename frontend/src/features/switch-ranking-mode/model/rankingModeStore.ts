import { create } from 'zustand';

import type { RankingMode } from '@/entities/manager';

interface RankingModeState {
  mode: RankingMode;
  setMode: (mode: RankingMode) => void;
}

export const useRankingModeStore = create<RankingModeState>((set) => ({
  mode: 'GrossProfit',
  setMode: (mode) => set({ mode }),
}));
