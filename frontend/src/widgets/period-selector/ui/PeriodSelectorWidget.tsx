import { CustomRangePicker, PeriodTabs } from '@/features/select-period';

export function PeriodSelectorWidget() {
  return (
    <div className="flex items-center gap-3">
      <PeriodTabs />
      <CustomRangePicker />
    </div>
  );
}
