import { ComponentChildren, ComponentType } from 'preact'
import styles from './Button.module.css'
import cn from 'classnames'
import { Spinner } from '@components/Spinner/Spinner.tsx'

export type ButtonProps = {
  title?: string
  appearance?: 'primary' | 'flat' | 'secondary' | 'outline'
  size?: 'medium'
  onClick?: () => void
  icon?: ComponentType
  disabled?: boolean
  type?: 'submit' | 'button' | 'label'
  children?: ComponentChildren
  loading?: boolean
  href?: string
}

export const Button = ({
  title,
  onClick,
  icon,
  disabled,
  appearance = 'primary',
  type = 'button',
  children,
  loading,
  href,
}: ButtonProps) => {
  // noinspection UnnecessaryLocalVariableJS
  const Icon = icon

  const content = (
    <>
      {!loading && Icon && <Icon />}
      {title && <span className={styles.text}>{title}</span>}
      {children}
      {loading && <Spinner size="small" color="var(--color-text)" />}
    </>
  )

  if (type === 'label') {
    return (
      <label
        className={cn(styles.button, { [styles[appearance]]: true, [styles.disabled]: disabled || loading })}
        onClick={onClick}
        aria-label={title}
      >
        {content}
      </label>
    )
  }

  if (href) {
    return (
      <a
        className={cn(styles.button, { [styles[appearance]]: true, [styles.disabled]: disabled || loading })}
        onClick={onClick}
        href={href}
      >
        {content}
      </a>
    )
  }

  return (
    <button
      className={cn(styles.button, styles[appearance])}
      onClick={onClick}
      disabled={disabled || loading}
      aria-label={title}
      type={type}
    >
      {content}
    </button>
  )
}
