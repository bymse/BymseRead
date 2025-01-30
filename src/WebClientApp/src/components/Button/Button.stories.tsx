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
    appearance: 'primary',
    size: 'medium',
    disabled: false,
  },
}

export const PrimaryIcon: Story = {
  args: {
    title: 'Button',
    appearance: 'primary',
    size: 'medium',
    disabled: false,
    icon: CheckIcon,
  },
}

export const Flat: Story = {
  args: {
    title: 'Button',
    appearance: 'flat',
    size: 'medium',
    disabled: false,
  },
}

export const Secondary: Story = {
  args: {
    title: 'Button',
    appearance: 'secondary',
    size: 'medium',
    disabled: false,
  },
}

export const Label: Story = {
  args: {
    title: 'Button',
    appearance: 'flat',
    size: 'medium',
    disabled: false,
    type: 'label',
    children: <input type="file" style={{ display: 'none' }} />,
  },
}

export const Outline: Story = {
  args: {
    title: 'Button',
    appearance: 'outline',
    size: 'medium',
    disabled: false,
    type: 'button',
  },
}
