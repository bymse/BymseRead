import { ComponentChild, ComponentChildren, FunctionalComponent, VNode } from 'preact'
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
    setIsOpen(wasOpen => {
      if (wasOpen && dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        return false
      }
      return wasOpen
    })
  }

  useEffect(() => {
    document.addEventListener('mouseup', handleClickOutside)
    return () => {
      document.removeEventListener('mouseup', handleClickOutside)
    }
  }, [])

  const handleToggle = () => setIsOpen(prev => !prev)
  const closeDropdown = () => setIsOpen(false)

  const mappedChildren =
    children instanceof Array
      ? children.map(child => {
          const node = child as VNode<unknown>
          if (node) return { ...node, props: { ...node.props, closeDropdown } }

          return child
        })
      : children

  const Button = button
  return (
    <div className={styles.dropdown} ref={dropdownRef}>
      <Button onClick={handleToggle} />
      {isOpen && <ul className={cn(styles.list, styles[side])}>{mappedChildren}</ul>}
    </div>
  )
}

type DropdownItemProps = {
  children: ComponentChildren
  color?: string
  onClick?: () => void
  closeDropdown?: () => void
}

export const DropdownItem: FunctionalComponent<DropdownItemProps> = ({
  children,
  color,
  onClick,
  closeDropdown,
}: DropdownItemProps) => {
  const handleClick = () => {
    if (onClick) onClick()
    if (closeDropdown) closeDropdown()
  }

  return (
    <li style={{ color }} className={styles.item} onClick={handleClick}>
      {children}
    </li>
  )
}
