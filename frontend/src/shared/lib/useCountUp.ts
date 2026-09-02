import { animate } from 'framer-motion';
import { useEffect, useRef, useState } from 'react';

/** Animates numeric text from its previous value to `target` whenever `target` changes. */
export function useCountUp(target: number, durationSec = 0.6): number {
  const [display, setDisplay] = useState(target);
  const previous = useRef(target);
  const isFirstRun = useRef(true);

  useEffect(() => {
    if (isFirstRun.current) {
      isFirstRun.current = false;
      previous.current = target;
      setDisplay(target);
      return;
    }

    const controls = animate(previous.current, target, {
      duration: durationSec,
      ease: 'easeOut',
      onUpdate: (value) => setDisplay(value),
    });

    previous.current = target;
    return () => controls.stop();
  }, [target, durationSec]);

  return display;
}
