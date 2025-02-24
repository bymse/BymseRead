import { Reader, ReaderProps } from './Reader.tsx'
import { Meta, StoryObj } from '@storybook/preact'

const meta: Meta<ReaderProps> = {
  component: Reader,
}
export default meta

type Story = StoryObj<ReaderProps>

export const Default: Story = {
  args: {
    pdfUrl: 'example.pdf',
  },
}
