import styles from './Input.module.css'
import { SearchIcon } from '@icons/SearchIcon.tsx'
import cn from 'classnames'

export type InputProps = {
  type: 'search' | 'text'
  placeholder?: string
  onSubmit?: (query: string) => void
  name?: string
  label?: string
  defaultValue?: string
}

export const Input = ({ type, placeholder, onSubmit, name, label, defaultValue }: InputProps) => {
  const handleKeyPress = (event: KeyboardEvent) => {
    if (event.key === 'Enter') {
      onSubmit?.((event.target as HTMLInputElement).value)
    }
  }

  const id = `input-${name}`
  return (
    <div className={styles.container}>
      {label && (
        <label className={styles.label} htmlFor={id}>
          {label}
        </label>
      )}
      <div className={cn(styles.inputWrapper, styles[type])}>
        {type === 'search' && (
          <div className={styles.icon}>
            <SearchIcon />
          </div>
        )}
        <input
          type={type}
          className={styles.input}
          placeholder={placeholder}
          onKeyPress={handleKeyPress}
          name={name}
          id={id}
          defaultValue={defaultValue}
        />
      </div>
    </div>
  )
}
