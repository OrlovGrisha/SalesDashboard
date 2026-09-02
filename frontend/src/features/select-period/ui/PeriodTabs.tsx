import { usePeriodStore } from '../model/periodStore';

import type { PeriodPreset } from '@/shared/api';
import { SegmentedControl, type SegmentedOption } from '@/shared/ui';

const options: SegmentedOption<PeriodPreset>[] = [
  { value: 'Today', label: 'Сегодня' },
  { value: 'Last7Days', label: '7 дней' },
  { value: 'Last30Days', label: '30 дней' },
  { value: 'ThisMonth', label: 'Этот месяц' },
  { value: 'LastMonth', label: 'Прошлый месяц' },
  { value: 'Custom', label: 'Период' },
];

export function PeriodTabs() {
  const preset = usePeriodStore((state) => state.preset);
  const setPreset = usePeriodStore((state) => state.setPreset);

  return (
    <SegmentedControl
      layoutId="period-tabs"
      options={options}
      value={preset}
      onChange={setPreset}
    />
  );
}
