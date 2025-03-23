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
}

export const Toast = ({ 
  message, 
  variant, 
  link, 
  onClose, 
  onClick, 
  position,
  linkText
}: ToastProps) => {
  return (
    <div className={cn(styles.toast, styles[variant], styles[position])}>
      <span className={cn(onClick && styles.button)} onClick={onClick}>
        {message}
      </span>
      {link && (
        <a href={link} rel="noopener noreferrer" className={styles.open}>
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
