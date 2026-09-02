import { screen } from '@testing-library/react';
import { beforeEach, describe, expect, it, vi } from 'vitest';

import { KpiSummaryWidget } from './KpiSummaryWidget';

import { apiClient } from '@/shared/api/client';
import { renderWithQueryClient } from '@/shared/config/test-utils';

vi.mock('@/shared/api/client', () => ({ apiClient: { get: vi.fn() } }));

describe('KpiSummaryWidget', () => {
  beforeEach(() => {
    vi.mocked(apiClient.get).mockReset();
  });

  it('shows loading skeletons first, then an error state with a retry button on failure', async () => {
    vi.mocked(apiClient.get).mockRejectedValue(new Error('Network error'));

    renderWithQueryClient(<KpiSummaryWidget />);

    expect(screen.getAllByRole('status').length).toBeGreaterThan(0);

    expect(await screen.findByText('Не удалось загрузить KPI')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Повторить' })).toBeInTheDocument();
  });
});
