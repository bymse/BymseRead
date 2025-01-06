import { Header } from './Header'
import { Meta, StoryObj } from '@storybook/preact'

const meta: Meta<typeof Header> = {
  component: Header,
  args: {},
}
export default meta

type Story = StoryObj<typeof Header>

export const Default: Story = {
  args: {},
}
