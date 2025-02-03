import { ComponentChildren, FunctionalComponent, createContext } from 'preact'
import styles from './Dropdown.module.css'
import { useEffect, useRef, useState, useContext } from 'preact/hooks'
import cn from 'classnames'
import { MoreHorIcon } from '@icons/MoreHorIcon.tsx'
import { Button } from '@components/Button/Button.tsx'

export interface DropdownButtonProps {
  onClick: () => void
}

export type DropdownProps = {
  children: ComponentChildren
  button?: FunctionalComponent<DropdownButtonProps>
  side: 'right' | 'left'
}

const DropdownContext = createContext<{ closeDropdown: () => void } | null>(null)

export const Dropdown: FunctionalComponent<DropdownProps> = ({ children, button, side }: DropdownProps) => {
  const [isOpen, setIsOpen] = useState(false)
  const dropdownRef = useRef<HTMLDivElement>(null)

  const closeDropdown = () => setIsOpen(false)

  const handleClickOutside = (event: MouseEvent) => {
    if (isOpen && dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
      closeDropdown()
    }
  }

  useEffect(() => {
    document.addEventListener('mouseup', handleClickOutside)
    return () => {
      document.removeEventListener('mouseup', handleClickOutside)
    }
  }, [isOpen])

  const handleToggle = () => setIsOpen(prev => !prev)

  // noinspection UnnecessaryLocalVariableJS
  const PassedButton = button

  return (
    <div className={styles.dropdown} ref={dropdownRef}>
      {PassedButton ? (
        <PassedButton onClick={handleToggle} />
      ) : (
        <Button icon={() => <MoreHorIcon color="var(--color-text-10)" />} appearance="flat" onClick={handleToggle} />
      )}
      {isOpen && (
        <DropdownContext.Provider value={{ closeDropdown }}>
          <ul className={cn(styles.list, styles[side])}>{children}</ul>
        </DropdownContext.Provider>
      )}
    </div>
  )
}

type DropdownItemProps = {
  children: ComponentChildren
  onClick?: () => void
}

export const DropdownItem: FunctionalComponent<DropdownItemProps> = ({ children, onClick }: DropdownItemProps) => {
  const context = useContext(DropdownContext)

  const handleClick = () => {
    if (onClick) onClick()
    if (context) context.closeDropdown()
  }

  return (
    <li className={styles.item} onClick={handleClick}>
      {children}
    </li>
  )
}
