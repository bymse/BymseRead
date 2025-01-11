import styles from './Header.module.scss'
import logo from '@assets/logo.svg'
import { Button } from '@components/Button/Button.tsx'
import { Input } from '@components/Input/Input.tsx'
import { SearchIcon } from '@icons/SearchIcon.tsx'
import { MoreHorIcon } from '@icons/MoreHorIcon.tsx'
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

  return (
    <header className={styles.header}>
      <div className={styles.logo}>
        <img alt="BymseRead" src={logo} />
      </div>
      <div className={styles.actions}>
        <Button title="Add new book" onClick={onAddBook} />
        <Input type="search" placeholder="Search" onSubmit={handleSearchSubmit} />
        <Button title="Log out" appearance="flat" onClick={onLogout} />
      </div>
      <div className={styles.actionsMobile}>
        <Button
          icon={() => <SearchIcon color="var(--color-text-10)" />}
          appearance="flat"
          onClick={onMobileSearchClick}
        />
        <Dropdown
          button={({ onClick }) => (
            <Button icon={() => <MoreHorIcon color="var(--color-text-10)" />} appearance="flat" onClick={onClick} />
          )}
          side="left"
        >
          <DropdownItem onClick={onAddBook}>Add new book</DropdownItem>
          <DropdownItem onClick={onLogout} color="var(--color-text-10)">
            Log out
          </DropdownItem>
        </Dropdown>
      </div>
    </header>
  )
}
