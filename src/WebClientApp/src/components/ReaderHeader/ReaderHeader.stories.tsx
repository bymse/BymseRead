import { ReaderHeader, ReaderHeaderProps } from './ReaderHeader.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const WrappedHeader = (props: ReaderHeaderProps) => (
  <div style={{ border: '1px dotted lightgray' }}>
    <ReaderHeader {...props} />
  </div>
)

const meta: Meta<ReaderHeaderProps> = {
  component: WrappedHeader,
  args: {},
}
export default meta

type Story = StoryObj<ReaderHeaderProps>

export const Default: Story = {
  args: {
    title: 'Long book title long book title long book title long book title long book title',
    currentPage: 10,
    totalPages: 100,
    onBookmarkClick: fn(),
    onEditBook: fn(),
    onDeleteBook: fn(),
  },
}
