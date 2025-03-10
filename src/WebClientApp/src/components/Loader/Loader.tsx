import styles from './Loader.module.scss'
import { Spinner } from '@components/Spinner/Spinner.tsx'
import { ComponentChild } from 'preact'

type Props = {
  showSpinner: boolean
  text: ComponentChild
}

export const Loader = ({ showSpinner, text }: Props) => {
  return (
    <>
      {showSpinner && (
        <div className={styles.loader}>
          <Spinner color="var(--color-base-primary-normal)" />
          <span>{text}</span>
        </div>
      )}
    </>
  )
}
