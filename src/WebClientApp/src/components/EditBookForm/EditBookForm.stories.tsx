import { EditBookFormProps, EditBookForm } from './EditBookForm.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const meta: Meta<EditBookFormProps> = {
  component: EditBookForm,
  args: {},
}
export default meta

type Story = StoryObj<EditBookFormProps>

export const Create: Story = {
  args: {
    onSubmit: fn(),
    onCancel: fn(),
    coverUrl: 'stories-cover.jpg',
    bookId: '1',
    title: 'Book title',
    fileUrl: 'stories-book.pdf',
    fileName: 'stories-book',
  },
  parameters: {
    layout: 'centered',
  },
}
