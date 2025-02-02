import styles from './Toast.module.scss'
import cn from 'classnames'

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
        <button className={styles.toastClose} onClick={onClose}>
          &times;
        </button>
      )}
    </div>
  )
}
