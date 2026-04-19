import { Toast } from '@components/Toast/Toast.tsx'
import styles from './ReturnToPageToast.module.scss'

export type ReturnToPageToastProps = {
  page: number
  onReturn: () => void
  onClose: () => void
}

export const ReturnToPageToast = ({ page, onReturn, onClose }: ReturnToPageToastProps) => {
  return (
    <div className={styles.root}>
      <Toast
        message={`Return to page ${page}`}
        variant="primary"
        position="relative"
        onClick={onReturn}
        onClose={onClose}
      />
    </div>
  )
}
