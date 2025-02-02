import styles from './PopupForm.module.scss'
import { ComponentChildren } from 'preact'
import { Button } from '@components/Button/Button.tsx'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'

export type PopupFormProps = {
  title: string
  onCancel: () => void
  onSubmit: (form: FormData) => void
  children: ComponentChildren
  submitTitle: string
}

export const PopupForm = ({ title, onCancel, onSubmit, children, submitTitle }: PopupFormProps) => {
  const handleCancel = () => {
    onCancel()
  }

  const handleSubmit = (event: Event) => {
    event.preventDefault()
    const form = new FormData(event.target as HTMLFormElement)
    onSubmit(form)
  }

  return (
    <div className={styles.veil}>
      <div className={styles.popup}>
        <h2 className={styles.title}>{title}</h2>
        <div className={styles.closeButton}>
          <Button icon={RemoveIcon} onClick={handleCancel} appearance="flat" />
        </div>
        <form onSubmit={handleSubmit} className={styles.form}>
          <div className={styles.formContent}>{children}</div>
          <div className={styles.formActions}>
            <Button type="submit" appearance="primary" title={submitTitle} />
            <Button onClick={handleCancel} appearance="secondary" title="Cancel" />
          </div>
        </form>
      </div>
    </div>
  )
}
