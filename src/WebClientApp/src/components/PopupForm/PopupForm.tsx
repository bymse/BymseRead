import { useState } from 'preact/hooks'
import styles from './PopupForm.module.scss'
import { ComponentChildren } from 'preact'
import { Button } from '@components/Button/Button.tsx'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'

export type PopupFormProps = {
  title: string
  onCancel: () => void
  onSubmit: (form: FormData) => void | Promise<void>
  children: ComponentChildren
  submitTitle: string
}

export const PopupForm = ({ title, onCancel, onSubmit, children, submitTitle }: PopupFormProps) => {
  const [isLoading, setIsLoading] = useState(false)
  const handleCancel = () => {
    onCancel()
  }

  const handleSubmit = async (event: Event) => {
    event.preventDefault()
    const form = new FormData(event.target as HTMLFormElement)
    try {
      setIsLoading(true)
      await onSubmit(form)
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className={styles.veil}>
      <div className={styles.popup}>
        <h2 className={styles.title}>{title}</h2>
        <div className={styles.closeButton}>
          <Button icon={RemoveIcon} onClick={handleCancel} appearance="flat" disabled={isLoading} />
        </div>
        <form onSubmit={e => void handleSubmit(e)} className={styles.form}>
          <fieldset disabled={isLoading}>
            <div className={styles.formContent}>{children}</div>
            <div className={styles.formActions}>
              <Button type="submit" appearance="primary" title={submitTitle} />
              <Button onClick={handleCancel} appearance="secondary" title="Cancel" />
            </div>
          </fieldset>
        </form>
      </div>
    </div>
  )
}
