import { Reader, ReaderProps } from './Reader.tsx'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'
import { Input } from '@components/Input/Input.tsx'
import { useState } from 'preact/hooks'

const Component = (props: ReaderProps) => {
  const [currentPage, setCurrentPage] = useState(props.currentPage)

  return (
    <>
      <div style={{ position: 'absolute', top: 5, left: 5, zIndex: 1000 }}>
        <Input type="text" defaultValue={currentPage?.toString()} onSubmit={e => setCurrentPage(parseInt(e))} />
        <br />
      </div>
      <Reader {...props} currentPage={currentPage} />
    </>
  )
}

const meta: Meta<ReaderProps> = {
  component: Component,
}
export default meta

type Story = StoryObj<ReaderProps>

export const Default: Story = {
  args: {
    pdfUrl: 'example.pdf',
    onCurrentPageChange: fn(),
    currentPage: 4,
  },
}
