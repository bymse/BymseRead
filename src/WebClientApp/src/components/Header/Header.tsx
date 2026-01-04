import styles from './Header.module.scss'
import logo from '@assets/logo.svg?inline'
import { Button } from '@components/Button/Button.tsx'
import { Input } from '@components/Input/Input.tsx'
import { SearchIcon } from '@icons/SearchIcon.tsx'
import { Dropdown, DropdownItem } from '@components/Dropdown/Dropdown.tsx'

export type HeaderProps = {
  onAddBook?: () => void
  onLogout?: () => void
  onSearchSubmit?: (query: string) => void
  onMobileSearchClick?: () => void
}

export const Header = ({ onAddBook, onLogout, onSearchSubmit, onMobileSearchClick }: HeaderProps) => {
  const handleSearchSubmit = (searchQuery: string) => {
    onSearchSubmit?.(searchQuery)
  }

  const showDropdown = onAddBook || onLogout

  return (
    <header className={styles.header}>
      <a className={styles.logo} href="/">
        <img alt="BymseRead" src={logo} />
      </a>
      <div className={styles.actions}>
        {onAddBook && <Button title="Add new book" onClick={onAddBook} data-testid="header-add-book-button" />}
        {onSearchSubmit && <Input type="search" placeholder="Search" onSubmit={handleSearchSubmit} />}
        {onLogout && <Button title="Log out" appearance="flat" onClick={onLogout} />}
      </div>
      <div className={styles.actionsMobile}>
        {onMobileSearchClick && (
          <Button
            icon={() => <SearchIcon color="var(--color-text-10)" />}
            appearance="flat"
            onClick={onMobileSearchClick}
          />
        )}
        {showDropdown && (
          <Dropdown side="left">
            {onAddBook && <DropdownItem onClick={onAddBook}>Add new book</DropdownItem>}
            {onLogout && <DropdownItem onClick={onLogout}>Log out</DropdownItem>}
          </Dropdown>
        )}
      </div>
    </header>
  )
}
