import styles from './Header.module.css'
import logo from '@assets/logo.svg'
import { Button } from '@components/Button/Button.tsx'
import { Input } from '@components/Input/Input.tsx'

export type HeaderProps = {
  onAddBook?: () => void
  onLogout?: () => void
  onSearchSubmit?: (query: string) => void
}

export const Header = ({ onAddBook, onLogout, onSearchSubmit }: HeaderProps) => {
  const handleSearchSubmit = (searchQuery: string) => {
    onSearchSubmit?.(searchQuery)
  }

  return (
    <header className={styles.header}>
      <div className={styles.logo}>
        <img width={116} height={50} alt="BymseRead" src={logo} />
      </div>
      <div className={styles.actions}>
        <Button title="Add new book" onClick={onAddBook} />
        <Input type="search" placeholder="Search" onSubmit={handleSearchSubmit} />
        <Button title="Log out" appearance="flat" onClick={onLogout} />
      </div>
    </header>
  )
}
