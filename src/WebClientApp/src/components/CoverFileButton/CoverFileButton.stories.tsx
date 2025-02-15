import { CoverFileButton, CoverFileButtonProps } from './CoverFileButton.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

type Props = {
  onSubmit: (args: string[]) => void
} & CoverFileButtonProps

const Wrapper = (props: Props) => {
  return (
    <form
      onSubmit={e => {
        e.preventDefault()
        props.onSubmit([...new FormData(e.target as HTMLFormElement).keys()])
      }}
    >
      <CoverFileButton {...props} />
      <button type="submit">Submit</button>
    </form>
  )
}

const meta: Meta<Props> = {
  component: Wrapper,
  args: {},
}
export default meta

type Story = StoryObj<Props>

export const Add: Story = {
  args: {
    inputName: 'coverFile',
    removedCheckName: 'coverRemoved',
    onSubmit: fn(),
  },
}

export const Edit: Story = {
  args: {
    coverUrl: 'stories-cover.png',
    inputName: 'coverFile',
    removedCheckName: 'coverRemoved',
    onSubmit: fn(),
  },
}
