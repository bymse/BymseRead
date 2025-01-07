import { Button, ButtonProps } from './Button'
import { Meta, StoryObj } from '@storybook/preact'
import { CheckIcon } from '@icons/CheckIcon.tsx'

const meta: Meta<ButtonProps> = {
  component: Button,
}
export default meta

type Story = StoryObj<ButtonProps>

export const Primary: Story = {
  args: {
    title: 'Button',
    variant: 'primary',
    size: 'medium',
    disabled: false,
  },
}

export const PrimaryIcon: Story = {
  args: {
    title: 'Button',
    variant: 'primary',
    size: 'medium',
    disabled: false,
    icon: CheckIcon,
  },
}
