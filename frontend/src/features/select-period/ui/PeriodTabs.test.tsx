import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { beforeEach, describe, expect, it, vi } from 'vitest';

import { usePeriodStore } from '../model/periodStore';

import { PeriodTabs } from './PeriodTabs';

import { apiClient } from '@/shared/api/client';
import { renderWithQueryClient } from '@/shared/config/test-utils';
import { KpiSummaryWidget } from '@/widgets/kpi-summary';

vi.mock('@/shared/api/client', () => ({ apiClient: { get: vi.fn() } }));

const kpiResponse = {
  data: {
    revenue: 100,
    grossProfit: 40,
    margin: 0.4,
    salesCount: 5,
    averageCheck: 20,
    topManagerName: 'Иван Иванов',
    comparison: {
      revenueChangePercent: 0,
      grossProfitChangePercent: 0,
      salesCountChangePercent: 0,
      averageCheckChangePercent: 0,
    },
  },
};

describe('PeriodTabs', () => {
  beforeEach(() => {
    usePeriodStore.setState({ preset: 'Last30Days', customFrom: null, customTo: null });
    vi.mocked(apiClient.get).mockReset();
    vi.mocked(apiClient.get).mockResolvedValue(kpiResponse);
  });

  it('switches the period preset and refetches KPI with the matching query params', async () => {
    renderWithQueryClient(
      <>
        <PeriodTabs />
        <KpiSummaryWidget />
      </>,
    );

    await waitFor(() =>
      expect(apiClient.get).toHaveBeenCalledWith('/analytics/kpi', {
        params: { preset: 'Last30Days' },
      }),
    );

    await userEvent.click(screen.getByRole('button', { name: 'Сегодня' }));

    await waitFor(() =>
      expect(apiClient.get).toHaveBeenCalledWith('/analytics/kpi', {
        params: { preset: 'Today' },
      }),
    );
  });
});
