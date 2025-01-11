import { ComponentChildren, FunctionalComponent } from 'preact'
import styles from './Dropdown.module.css'
import { useEffect, useRef, useState } from 'preact/hooks'
import cn from 'classnames'

export interface DropdownButtonProps {
  onClick: () => void
}

export type DropdownProps = {
  children: ComponentChildren
  button: FunctionalComponent<DropdownButtonProps>
  side: 'right' | 'left'
}

export const Dropdown: FunctionalComponent<DropdownProps> = ({ children, button, side }: DropdownProps) => {
  const [isOpen, setIsOpen] = useState(false)
  const dropdownRef = useRef<HTMLDivElement>(null)

  const handleClickOutside = (event: MouseEvent) => {
    if (isOpen && dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
      setIsOpen(false)
    }
  }

  useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside)
    return () => {
      document.removeEventListener('mousedown', handleClickOutside)
    }
  }, [])

  const handleToggle = () => setIsOpen(r => !r)
  const Button = button
  return (
    <div className={styles.dropdown} ref={dropdownRef}>
      <Button onClick={handleToggle} />
      {isOpen && <ul className={cn(styles.list, styles[side])}>{children}</ul>}
    </div>
  )
}

type DropdownItemProps = {
  children: ComponentChildren
  color?: string
  onClick?: () => void
}

export const DropdownItem: FunctionalComponent<DropdownItemProps> = ({
  children,
  color,
  onClick,
}: DropdownItemProps) => {
  return (
    <li style={{ color: color }} className={styles.item} onClick={onClick}>
      {children}
    </li>
  )
}
