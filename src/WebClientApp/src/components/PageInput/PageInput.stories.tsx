import { PageInput, PageInputProps } from './PageInput.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const WrappedInput = (props: PageInputProps) => (
  <span style={{ border: '1px dotted lightgray', padding: '10px' }}>
    <PageInput {...props} />
  </span>
)

const meta: Meta<PageInputProps> = {
  component: WrappedInput,
  args: {},
}
export default meta

type Story = StoryObj<PageInputProps>

export const Default: Story = {
  args: {
    defaultValue: 10,
    maxNumber: 100,
    onBlur: fn(),
  },
}
