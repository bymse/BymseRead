import { createContext, ComponentChildren } from 'preact'
import { useState, useCallback, useRef, useContext } from 'preact/hooks'
import { Toast, ToastVariant } from './Toast'

interface ToastState {
  message: string
  variant: ToastVariant
  visible: boolean
  link?: string
  linkText?: string
}

interface ToastContextType {
  showError: (message: string, link?: string, duration?: number, actionText?: string) => void
  showInfo: (message: string, link?: string, duration?: number) => void
}

export const useToast = () => useContext(ToastContext)

const ToastContext = createContext<ToastContextType>({
  showError: () => {},
  showInfo: () => {},
})

interface ToastProviderProps {
  children: ComponentChildren
}

export const ToastProvider = ({ children }: ToastProviderProps) => {
  const [toast, setToast] = useState<ToastState>({
    message: '',
    variant: 'info',
    visible: false,
  })
  const timeoutRef = useRef<number | null>(null)

  const hideToast = useCallback(() => {
    setToast(prev => ({ ...prev, visible: false }))
    if (timeoutRef.current !== null) {
      clearTimeout(timeoutRef.current)
      timeoutRef.current = null
    }
  }, [])

  const showError = useCallback(
    (message: string, link?: string, duration: number = 3000, linkText?: string) => {
      const truncated = message.length > 70 ? `${message.slice(0, 70)}...` : message
      setToast({ message: truncated, variant: 'error', visible: true, link, linkText })
      if (timeoutRef.current !== null) {
        clearTimeout(timeoutRef.current)
      }

      if (duration) {
        timeoutRef.current = window.setTimeout(() => {
          hideToast()
        }, duration)
      }
    },
    [hideToast],
  )

  const showInfo = useCallback(
    (message: string, link?: string, duration: number = 3000) => {
      setToast({ message, variant: 'info', visible: true, link })
      if (timeoutRef.current !== null) {
        clearTimeout(timeoutRef.current)
      }
      timeoutRef.current = window.setTimeout(() => {
        hideToast()
      }, duration)
    },
    [hideToast],
  )

  return (
    <ToastContext.Provider value={{ showError, showInfo }}>
      {children}
      {toast.visible && (
        <Toast
          {...toast}
          onClose={toast.variant === 'error' && !toast.link ? hideToast : undefined}
          hideToast={hideToast}
          position="top"
        />
      )}
    </ToastContext.Provider>
  )
}
