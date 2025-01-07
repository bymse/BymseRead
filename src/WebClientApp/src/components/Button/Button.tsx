import { ComponentType } from 'preact'
import styles from './Button.module.css'
import cn from 'classnames'

export type ButtonProps = {
  title: string
  appearance?: 'primary' | 'flat'
  size?: 'medium'
  onClick?: () => void
  icon?: ComponentType
  disabled?: boolean
}

export const Button = ({ title, onClick, icon, disabled, appearance = 'primary' }: ButtonProps) => {
  // noinspection UnnecessaryLocalVariableJS
  const Icon = icon
  return (
    <button className={cn(styles.Button, styles[appearance])} onClick={onClick} disabled={disabled} aria-label={title}>
      {Icon && <Icon />}
      <span className={styles.Text}>{title}</span>
    </button>
  )
}
