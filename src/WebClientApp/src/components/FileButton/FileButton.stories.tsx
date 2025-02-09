import { FileButton, FileButtonProps } from './FileButton'
import { Meta, StoryObj } from '@storybook/preact'

const WrappedButton = (props: FileButtonProps) => (
  <div style={{ width: '400px' }}>
    <FileButton {...props} />
  </div>
)

const meta: Meta<FileButtonProps> = {
  component: WrappedButton,
}
export default meta

type Story = StoryObj<FileButtonProps>

export const New: Story = {
  args: {
    inputName: 'file',
  },
}

export const Existing: Story = {
  args: {
    fileName: 'file.pdf',
    fileUrl: 'example.pdf',
    inputName: 'file',
  },
}
