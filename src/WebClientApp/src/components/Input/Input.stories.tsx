﻿import { Input, InputProps } from './Input'
import { Meta, StoryObj } from '@storybook/preact'

const meta: Meta<InputProps> = {
  component: Input,
}
export default meta

type Story = StoryObj<InputProps>

export const Search: Story = {
  args: {
    type: 'search',
    placeholder: 'Placeholder',
  },
}

export const Text: Story = {
  args: {
    type: 'text',
    placeholder: 'Placeholder',
    label: 'Title',
    name: 'text',
  },
}
