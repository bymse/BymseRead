import styles from './Toast.module.scss'
import cn from 'classnames'
import { CloseIcon } from '@icons/CloseIcon.tsx'

export type ToastVariant = 'info' | 'error'

export type ToastProps = {
  message: string
  variant: ToastVariant
  link?: string
  onClose?: () => void
  onClick?: () => void
  position: 'top' | 'relative'
  linkText?: string
  hideToast?: () => void
}

export const Toast = ({ message, variant, link, onClose, onClick, position, linkText, hideToast }: ToastProps) => {
  return (
    <div className={cn(styles.toast, styles[variant], styles[position])} data-testid="toast">
      <span className={cn(onClick && styles.button)} onClick={onClick}>
        {message}
      </span>
      {link && (
        <a href={link} rel="noopener noreferrer" className={styles.open} onClick={hideToast} data-testid="toast-link">
          {linkText || 'Open'}
        </a>
      )}
      {onClose && (
        <span className={styles.close} onClick={onClose}>
          <CloseIcon color="currentColor" />
        </span>
      )}
    </div>
  )
}
