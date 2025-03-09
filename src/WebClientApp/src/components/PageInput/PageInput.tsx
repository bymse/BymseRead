import { useState, useEffect, useRef } from 'react'
import styles from './PageInput.module.css'

export type PageInputProps = {
  defaultValue?: number
  maxNumber?: number
  onValueChange?: (value: number) => void
}

export const PageInput = ({ defaultValue, maxNumber, onValueChange }: PageInputProps) => {
  const [inputWidth, setInputWidth] = useState('auto')
  const inputRef = useRef<HTMLInputElement>(null)

  const updateWidth = () => {
    if (inputRef.current) {
      const length = (inputRef.current.value || defaultValue?.toString() || '').length
      const width = length === 0 ? 1 : length > 5 ? 5 : length
      setInputWidth(`${width}ch`)
    }
  }

  const updateValue = (event: Event) => {
    const inputValue = parseInt((event.target as HTMLInputElement).value, 10)
    const value = isNaN(inputValue) ? 1 : maxNumber && inputValue > maxNumber ? maxNumber : inputValue
    if (inputRef.current) {
      inputRef.current.value = value.toString()
      updateWidth()
    }
    if (onValueChange) {
      onValueChange(value)
    }
  }

  const handleBlur = (event: FocusEvent) => {
    updateValue(event)
  }

  const handleKeyDown = (event: KeyboardEvent) => {
    if (event.key === 'Enter') {
      updateValue(event)
    }
  }

  useEffect(() => {
    if (defaultValue && inputRef.current) {
      inputRef.current.value = defaultValue?.toString()
    }
    updateWidth()
  }, [defaultValue])

  return (
    <label className={styles.wrapper}>
      <input
        ref={inputRef}
        className={styles.input}
        type="number"
        defaultValue={defaultValue}
        onBlur={handleBlur}
        onKeyDown={handleKeyDown}
        max={maxNumber}
        onInput={updateWidth}
        style={{ width: inputWidth }}
        placeholder={defaultValue?.toString()}
      />
    </label>
  )
}
