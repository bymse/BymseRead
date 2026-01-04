import { useState } from 'preact/hooks'
import styles from './PopupForm.module.scss'
import { ComponentChildren } from 'preact'
import { Button } from '@components/Button/Button.tsx'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'
import cn from 'classnames'

export type PopupFormProps = {
  title: string
  onCancel: () => void
  onSubmit: (form: FormData) => void | Promise<void>
  children?: ComponentChildren
  submitTitle: string
  noClose?: boolean
  'data-testid'?: string
}

export const PopupForm = ({
  title,
  onCancel,
  onSubmit,
  children,
  submitTitle,
  noClose,
  'data-testid': dataTestId,
}: PopupFormProps) => {
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

  const isSmall = !children

  return (
    <div className={styles.veil}>
      <div
        className={cn({
          [styles.popup]: true,
          [styles.small]: isSmall,
        })}
        data-testid={dataTestId}
      >
        <h2 className={styles.title}>{title}</h2>
        {!noClose && (
          <div className={styles.closeButton}>
            <Button icon={RemoveIcon} onClick={handleCancel} appearance="flat" disabled={isLoading} />
          </div>
        )}
        <form onSubmit={e => void handleSubmit(e)} className={styles.form}>
          <fieldset disabled={isLoading}>
            {!isSmall && <div className={styles.formContent}>{children}</div>}
            <div className={styles.formActions}>
              <Button
                type="submit"
                appearance="primary"
                title={submitTitle}
                loading={isLoading}
                data-testid="popup-form-submit"
              />
              <Button onClick={handleCancel} appearance="secondary" title="Cancel" />
            </div>
          </fieldset>
        </form>
      </div>
    </div>
  )
}
