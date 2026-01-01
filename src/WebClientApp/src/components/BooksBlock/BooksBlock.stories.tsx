import { BooksBlock, BooksBlockProps } from './BooksBlock'
import { Meta, StoryObj } from '@storybook/preact'

const meta: Meta<BooksBlockProps> = {
  component: BooksBlock,
  args: {},
  parameters: {
    backgrounds: {
      default: 'Light',
      values: [{ name: 'Light', value: '#F2F3F8' }],
    },
  },
}
export default meta

type Story = StoryObj<BooksBlockProps>

export const Default: Story = {
  args: {
    title: 'Active',
    books: [
      {
        bookId: '1',
        title: 'Гении и аутсайдеры. Почему одним все, а другим ничего?',
        coverUrl: 'stories-cover.jpg',
        fileUrl: 'stories-file.epub',
        currentPage: 23,
        pages: 100,
        lastBookmark: null,
      },
      {
        bookId: '2',
        title: 'К себе нежно. Книга о том, как ценить и беречь себя',
        coverUrl: 'stories-cover.jpg',
        fileUrl: 'stories-file.epub',
        currentPage: 50,
        pages: 100,
        lastBookmark: null,
      },
      {
        bookId: '3',
        title:
          'Парадоксы эволюции. Как наличие ресурсов и отсутствие внешних угроз приводит к самоуничтожению вида и что мы можем с этим сделать',
        coverUrl: 'stories-cover.jpg',
        fileUrl: 'stories-file.epub',
        currentPage: 30,
        pages: 100,
        lastBookmark: null,
      },
      {
        bookId: '4',
        title: 'Исчезающая ложка. Тайны периодической таблицы Менделеева',
        coverUrl: 'stories-cover.jpg',
        fileUrl: 'stories-file.epub',
        currentPage: 60,
        pages: 100,
        lastBookmark: null,
      },
      {
        bookId: '5',
        title: 'Космос',
        coverUrl: 'stories-cover.jpg',
        fileUrl: 'stories-file.epub',
        currentPage: 80,
        pages: 100,
        lastBookmark: null,
      },
    ],
  },
}
