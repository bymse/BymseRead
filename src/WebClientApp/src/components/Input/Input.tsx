import styles from './Input.module.css'
import { SearchIcon } from '@icons/SearchIcon.tsx'
import cn from 'classnames'

export type InputProps = {
  type: 'search' | 'text'
  placeholder?: string
  onSubmit?: (query: string) => void
}

export const Input = ({ type, placeholder, onSubmit }: InputProps) => {
  const handleKeyPress = (event: KeyboardEvent) => {
    if (event.key === 'Enter') {
      onSubmit?.((event.target as HTMLInputElement).value)
    }
  }

  return (
    <div className={cn(styles.wrapper, styles[type])}>
      {type === 'search' && (
        <div className={styles.icon}>
          <SearchIcon />
        </div>
      )}
      <input type={type} className={styles.input} placeholder={placeholder} onKeyPress={handleKeyPress} />
    </div>
  )
}
