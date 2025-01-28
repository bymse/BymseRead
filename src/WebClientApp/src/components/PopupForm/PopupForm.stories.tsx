import { PopupFormProps, PopupForm } from './PopupForm.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'
import { Input } from '@components/Input/Input.tsx'

const meta: Meta<PopupFormProps> = {
  component: PopupForm,
  args: {},
}
export default meta

type Story = StoryObj<PopupFormProps>

export const Default: Story = {
  args: {
    title: 'Title',
    onSubmit: fn(),
    onCancel: fn(),
    submitTitle: 'Submit',
    children: (
      <>
        <Input type="search" />
      </>
    ),
  },
  parameters: {
    layout: 'centered',
  },
}
