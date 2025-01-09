import styles from './Input.module.css'
import { SearchIcon } from '@icons/SearchIcon.tsx'

export type InputProps = {
  type: 'search'
  placeholder?: string
}

export const Input = ({ type, placeholder }: InputProps) => {
  return (
    <div className={styles.wrapper}>
      {type === 'search' && (
        <div className={styles.icon}>
          <SearchIcon />
        </div>
      )}
      <input type={type} className={styles.input} placeholder={placeholder} />
    </div>
  )
}
