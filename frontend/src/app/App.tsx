import { QueryProvider } from './providers/QueryProvider';

import { DashboardPage } from '@/pages/dashboard';

export function App() {
  return (
    <QueryProvider>
      <DashboardPage />
    </QueryProvider>
  );
}
