import { useState, useCallback } from 'preact/hooks'

export const useShowHide = (): { visible: boolean; open: () => void; close: () => void } => {
  const [visible, setVisible] = useState(false)
  const open = useCallback(() => setVisible(true), [])
  const close = useCallback(() => setVisible(false), [])
  return { visible, open, close }
}
