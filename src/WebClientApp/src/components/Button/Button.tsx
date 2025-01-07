import { ComponentType } from 'preact'
import styles from './Button.module.css'
import { CheckIcon } from '@icons/CheckIcon.tsx'

export type ButtonProps = {
  title: string
  variant?: 'primary'
  size?: 'medium'
  onClick?: () => void
  icon?: ComponentType
  disabled?: boolean
}

export const Button = ({ title, onClick, icon = CheckIcon, disabled }: ButtonProps) => {
  const Icon = icon
  return (
    <button className={styles.Button} onClick={onClick} disabled={disabled} aria-label={title}>
      {icon && <Icon />}
      <span className={styles.Text}>{title}</span>
    </button>
  )
}
