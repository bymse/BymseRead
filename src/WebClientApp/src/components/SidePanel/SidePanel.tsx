import { ComponentChildren } from 'preact'
import { Button } from '@components/Button/Button.tsx'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'
import styles from './SidePanel.module.scss'
import cn from 'classnames'

export type SidePanelProps = {
  title: ComponentChildren
  onClose: () => void
  children: ComponentChildren
  testId?: string
  closeButtonTestId?: string
  className?: string
}

export const SidePanel = ({ title, onClose, children, testId, closeButtonTestId, className }: SidePanelProps) => {
  return (
    <div className={cn(styles.container, className)} data-testid={testId}>
      <div className={styles.header}>
        <h2 className={styles.title}>{title}</h2>
        <div className={styles.close}>
          <Button icon={RemoveIcon} onClick={onClose} appearance="flat" data-testid={closeButtonTestId} />
        </div>
      </div>
      <div className={styles.content}>{children}</div>
    </div>
  )
}
