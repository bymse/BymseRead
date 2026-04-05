import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'
import { ContentsPanel, ContentsPanelProps } from './ContentsPanel.tsx'
import { ReaderOutlineItem } from '@components/Reader/readerOutline.ts'

const baseItems: ReaderOutlineItem[] = [
  {
    id: '0',
    title: 'Foreword',
    pageNumber: 6,
    url: null,
    children: [],
    depth: 0,
    isExternal: false,
    isNavigable: true,
  },
  {
    id: '1',
    title: 'Chapter 2. Introducing Cassandra',
    pageNumber: 42,
    url: null,
    children: [
      {
        id: '1-0',
        title: 'The Cassandra Elevator Pitch',
        pageNumber: 42,
        url: null,
        children: [
          {
            id: '1-0-0',
            title: 'Cassandra in 50 Words or Less',
            pageNumber: 43,
            url: null,
            children: [],
            depth: 2,
            isExternal: false,
            isNavigable: true,
          },
          {
            id: '1-0-1',
            title: 'Distributed and Decentralized',
            pageNumber: 43,
            url: null,
            children: [],
            depth: 2,
            isExternal: false,
            isNavigable: true,
          },
        ],
        depth: 1,
        isExternal: false,
        isNavigable: true,
      },
      {
        id: '1-1',
        title: 'Where Did Cassandra Come From?',
        pageNumber: 60,
        url: null,
        children: [],
        depth: 1,
        isExternal: false,
        isNavigable: true,
      },
    ],
    depth: 0,
    isExternal: false,
    isNavigable: true,
  },
  {
    id: '2',
    title: 'Project website',
    pageNumber: null,
    url: 'https://example.com',
    children: [],
    depth: 0,
    isExternal: true,
    isNavigable: true,
  },
  {
    id: '3',
    title: 'Appendix without destination',
    pageNumber: null,
    url: null,
    children: [],
    depth: 0,
    isExternal: false,
    isNavigable: false,
  },
]

const meta: Meta<ContentsPanelProps> = {
  component: ContentsPanel,
  args: {
    items: baseItems,
    currentPage: 43,
    onNavigate: fn(),
    onClose: fn(),
  },
  decorators: [
    Story => (
      <div style={{ height: '100vh', position: 'relative', background: '#edf0f6' }}>
        <Story />
      </div>
    ),
  ],
}

export default meta

type Story = StoryObj<ContentsPanelProps>

export const FlatContents: Story = {
  args: {
    items: baseItems.slice(0, 2).map(item => ({ ...item, children: [] })),
    currentPage: 42,
  },
}

export const NestedContents: Story = {}

export const DisabledAndExternal: Story = {
  args: {
    items: [baseItems[2], baseItems[3]],
    currentPage: 1,
  },
}

export const ActiveRowAndExpandedPath: Story = {}
