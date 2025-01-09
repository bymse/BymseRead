import styles from './Input.module.css'
import { SearchIcon } from '@icons/SearchIcon.tsx'

export type InputProps = {
  type: 'search'
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
    <div className={styles.wrapper}>
      {type === 'search' && (
        <div className={styles.icon}>
          <SearchIcon />
        </div>
      )}
      <input type={type} className={styles.input} placeholder={placeholder} onKeyPress={handleKeyPress} />
    </div>
  )
}
