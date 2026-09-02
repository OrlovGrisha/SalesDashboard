import { screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { beforeEach, describe, expect, it, vi } from 'vitest';

import { ManagerRankingWidget } from './ManagerRankingWidget';

import { useRankingModeStore } from '@/features/switch-ranking-mode';
import { apiClient } from '@/shared/api/client';
import { renderWithQueryClient } from '@/shared/config/test-utils';

vi.mock('@/shared/api/client', () => ({ apiClient: { get: vi.fn() } }));

describe('ManagerRankingWidget', () => {
  beforeEach(() => {
    useRankingModeStore.setState({ mode: 'GrossProfit' });
    vi.mocked(apiClient.get).mockReset();
    vi.mocked(apiClient.get).mockResolvedValue({ data: [] });
  });

  it('switches the ranking mode and refetches with the matching mode param', async () => {
    renderWithQueryClient(<ManagerRankingWidget />);

    await waitFor(() =>
      expect(apiClient.get).toHaveBeenCalledWith('/managers/ranking', {
        params: { preset: 'Last30Days', mode: 'GrossProfit' },
      }),
    );

    await userEvent.click(screen.getByRole('button', { name: 'По среднему чеку' }));

    await waitFor(() =>
      expect(apiClient.get).toHaveBeenCalledWith('/managers/ranking', {
        params: { preset: 'Last30Days', mode: 'AverageCheck' },
      }),
    );
  });
});
