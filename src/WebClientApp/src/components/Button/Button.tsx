import { ComponentChildren, ComponentType } from 'preact'
import styles from './Button.module.css'
import cn from 'classnames'

export type ButtonProps = {
  title?: string
  appearance?: 'primary' | 'flat' | 'secondary' | 'outline'
  size?: 'medium'
  onClick?: () => void
  icon?: ComponentType
  disabled?: boolean
  type?: 'submit' | 'button' | 'label'
  children?: ComponentChildren
}

export const Button = ({
  title,
  onClick,
  icon,
  disabled,
  appearance = 'primary',
  type = 'button',
  children,
}: ButtonProps) => {
  // noinspection UnnecessaryLocalVariableJS
  const Icon = icon

  if (type === 'label') {
    return (
      <label
        className={cn(styles.button, { [styles[appearance]]: true, [styles.disabled]: disabled })}
        onClick={onClick}
        aria-label={title}
      >
        {Icon && <Icon />}
        {title && <span className={styles.text}>{title}</span>}
        {children}
      </label>
    )
  }

  return (
    <button
      className={cn(styles.button, styles[appearance])}
      onClick={onClick}
      disabled={disabled}
      aria-label={title}
      type={type}
    >
      {Icon && <Icon />}
      {title && <span className={styles.text}>{title}</span>}
      {children}
    </button>
  )
}
