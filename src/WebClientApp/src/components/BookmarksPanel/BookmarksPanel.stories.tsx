import { BookmarksPanel, BookmarksPanelProps } from './BookmarksPanel.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const meta: Meta<BookmarksPanelProps> = {
  component: BookmarksPanel,
  args: {},
}
export default meta

type Story = StoryObj<BookmarksPanelProps>

export const Create: Story = {
  args: {
    onMarkAsLastPage: fn(),
    lastPageDate: new Date(),
    lastPage: 23,
    onClose: fn(),
    onLastPageClick: fn(),
    currentPage: 50,
    onReturnToPageClick: fn(),
  },
}
