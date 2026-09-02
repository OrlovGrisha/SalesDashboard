import { motion } from 'framer-motion';

import { useCountUp } from '@/shared/lib';
import { Card, CardBody, DeltaIndicator } from '@/shared/ui';

const cardVariants = {
  hidden: { opacity: 0, y: 12 },
  visible: { opacity: 1, y: 0 },
};

interface KpiCardProps {
  label: string;
  value: number;
  format: (value: number) => string;
  delta?: number;
}

export function KpiCard({ label, value, format, delta }: KpiCardProps) {
  const animatedValue = useCountUp(value);

  return (
    <motion.div variants={cardVariants} transition={{ duration: 0.25 }}>
      <Card>
        <CardBody>
          <p className="text-xs font-medium text-text-secondary">{label}</p>
          <p className="mt-1.5 text-2xl font-semibold text-text-primary">{format(animatedValue)}</p>
          <div className="mt-2 h-4">
            {delta !== undefined && <DeltaIndicator percent={delta} />}
          </div>
        </CardBody>
      </Card>
    </motion.div>
  );
}

export function TopManagerCard({ name }: { name: string | null }) {
  return (
    <motion.div variants={cardVariants} transition={{ duration: 0.25 }}>
      <Card>
        <CardBody>
          <p className="text-xs font-medium text-text-secondary">Лучший менеджер</p>
          <p
            className="mt-1.5 truncate text-2xl font-semibold text-text-primary"
            title={name ?? undefined}
          >
            {name ?? '—'}
          </p>
          <div className="mt-2 h-4" />
        </CardBody>
      </Card>
    </motion.div>
  );
}
