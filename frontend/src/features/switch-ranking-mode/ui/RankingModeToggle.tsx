import { useRankingModeStore } from '../model/rankingModeStore';

import type { RankingMode } from '@/entities/manager';
import { SegmentedControl, type SegmentedOption } from '@/shared/ui';

const options: SegmentedOption<RankingMode>[] = [
  { value: 'GrossProfit', label: 'По прибыли' },
  { value: 'AverageCheck', label: 'По среднему чеку' },
];

export function RankingModeToggle() {
  const mode = useRankingModeStore((state) => state.mode);
  const setMode = useRankingModeStore((state) => state.setMode);

  return (
    <SegmentedControl layoutId="ranking-mode" options={options} value={mode} onChange={setMode} />
  );
}
