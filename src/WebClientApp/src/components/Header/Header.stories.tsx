import { Header, HeaderProps } from './Header'
import { Meta, StoryObj } from '@storybook/preact'
import { fn } from '@storybook/test'

const WrappedHeader = (props: HeaderProps) => (
  <div style={{ border: '1px dotted lightgray' }}>
    <Header {...props} />
  </div>
)

const meta: Meta<HeaderProps> = {
  component: WrappedHeader,
  args: {},
}
export default meta

type Story = StoryObj<HeaderProps>

export const Default: Story = {
  args: {
    onSearchSubmit: fn(),
    onMobileSearchClick: fn(),
    onLogout: fn(),
    onAddBook: fn(),
  },
}
