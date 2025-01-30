import { BookForm, BookFormProps } from './BookForm.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const meta: Meta<BookFormProps> = {
  component: BookForm,
  args: {},
}
export default meta

type Story = StoryObj<BookFormProps>

export const Create: Story = {
  args: {
    onSubmit: fn(),
    onCancel: fn(),
  },
  parameters: {
    layout: 'centered',
  },
}
