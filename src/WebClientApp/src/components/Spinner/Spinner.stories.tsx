import { Spinner, SpinnerProps } from './Spinner.tsx'
import { Meta, StoryObj } from '@storybook/preact'

const meta: Meta<SpinnerProps> = {
  component: Spinner,
  args: {},
}
export default meta

type Story = StoryObj<SpinnerProps>

export const Large: Story = {
  args: {
    size: 'large',
    color: 'var(--color-base-primary-normal)',
  },
}

export const Small: Story = {
  args: {
    size: 'small',
    color: 'var(--color-base-primary-normal)',
  },
}
