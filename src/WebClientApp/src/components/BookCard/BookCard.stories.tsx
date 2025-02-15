import { BookCard, BookCardProps } from './BookCard'
import { Meta, StoryObj } from '@storybook/preact'

const meta: Meta<BookCardProps> = {
  component: BookCard,
  args: {},
}
export default meta

type Story = StoryObj<BookCardProps>

export const NotStarted: Story = {
  args: {
    title: 'Гении и аутсайдеры',
    coverUrl: 'stories-cover.jpg',
  },
}

export const Started: Story = {
  args: {
    title: 'К себе нежно. Книга о том, как ценить и беречь себя',
    coverUrl: 'stories-cover.jpg',
    percentageFinished: 30,
  },
}

export const Finished: Story = {
  args: {
    title: 'Гении и аутсайдеры',
    coverUrl: 'stories-cover.jpg',
    percentageFinished: 100,
  },
}

export const NoCover: Story = {
  args: {
    title: 'Kingdom of the Wicked',
    percentageFinished: 50,
  },
}
