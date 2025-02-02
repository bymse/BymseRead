import { useContext } from 'preact/hooks'
import { Meta, StoryObj } from '@storybook/preact'
import { ToastProvider, ToastContext } from './ToastContext'
import { ToastProps } from './Toast.tsx'
import { Button } from '@components/Button/Button.tsx'

type Props = ToastProps & {
  duration?: number
}

const ToastStoryComponent = (props: Props) => {
  const Component = (p: Props) => {
    const { showError, showInfo } = useContext(ToastContext)
    const handleClick = () => {
      if (p.variant === 'error') {
        showError(p.message, p.duration)
      } else {
        showInfo(p.message, p.link, p.duration)
      }
    }
    return <Button onClick={handleClick} title="Show toast" />
  }

  return (
    <ToastProvider>
      <Component {...props} />
    </ToastProvider>
  )
}

export default {
  title: 'Components/Toast',
  component: ToastStoryComponent,
} as Meta

type Story = StoryObj<Props>

export const Error: Story = {
  args: {
    message: 'Error message',
    variant: 'error',
    duration: 3000,
  },
}

export const Info: Story = {
  args: {
    message: 'Info message',
    variant: 'info',
    duration: 3000,
    link: 'https://example.com',
  },
}
