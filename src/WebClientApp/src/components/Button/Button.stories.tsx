import { Button, ButtonProps } from './Button'
import { Meta, StoryObj } from '@storybook/preact'

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
