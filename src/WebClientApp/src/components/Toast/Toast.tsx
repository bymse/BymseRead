import styles from './Toast.module.scss'
import cn from 'classnames'
import { CloseIcon } from '@icons/CloseIcon.tsx'

export type ToastVariant = 'info' | 'error'

export type ToastProps = {
  message: string
  variant: ToastVariant
  link?: string
  onClose?: () => void
}

export const Toast = ({ message, variant, link, onClose }: ToastProps) => {
  return (
    <div className={cn(styles.toast, styles[variant])}>
      <span>{message}</span>
      {link && (
        <a href={link} target="_blank" rel="noopener noreferrer" className={styles.open}>
          Open
        </a>
      )}
      {!link && (
        <span className={styles.close} onClick={onClose}>
          <CloseIcon color="currentColor" />
        </span>
      )}
    </div>
  )
}
