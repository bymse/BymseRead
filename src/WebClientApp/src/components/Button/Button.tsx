import { ComponentType } from 'preact'
import styles from './Button.module.css'

export type ButtonProps = {
  title: string
  variant?: 'primary'
  size?: 'medium'
  onClick?: () => void
  icon?: ComponentType
  disabled?: boolean
}

export const Button = ({ title, onClick, icon, disabled }: ButtonProps) => {
  const Icon = icon
  return (
    <button className={styles.Button} onClick={onClick} disabled={disabled} aria-label={title}>
      {Icon && <Icon />}
      <span className={styles.Text}>{title}</span>
    </button>
  )
}
