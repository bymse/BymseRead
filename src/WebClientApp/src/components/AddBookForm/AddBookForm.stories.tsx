import { AddBookForm, AddBookFormProps } from './AddBookForm.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const meta: Meta<AddBookFormProps> = {
  component: AddBookForm,
  args: {},
}
export default meta

type Story = StoryObj<AddBookFormProps>

export const Create: Story = {
  args: {
    onSubmit: fn(),
    onCancel: fn(),
  },
  parameters: {
    layout: 'centered',
  },
}
