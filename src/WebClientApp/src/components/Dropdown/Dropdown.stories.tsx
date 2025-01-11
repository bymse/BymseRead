import { Dropdown, DropdownItem, DropdownProps } from './Dropdown'
import { Meta, StoryObj } from '@storybook/preact'
import { Button } from '@components/Button/Button.tsx'
import { MoreHorIcon } from '@icons/MoreHorIcon.tsx'

const meta: Meta<DropdownProps> = {
  component: Dropdown,
  args: {},
}
export default meta

type Story = StoryObj<DropdownProps>

export const Left: Story = {
  args: {
    side: 'left',
    button: ({ onClick }) => <Button appearance="flat" icon={MoreHorIcon} onClick={onClick} />,
    children: (
      <>
        <DropdownItem>text</DropdownItem>
        <DropdownItem>text</DropdownItem>
      </>
    ),
  },
  parameters: {
    layout: 'centered',
  },
}

export const Right: Story = {
  args: {
    side: 'right',
    button: ({ onClick }) => <Button appearance="flat" icon={MoreHorIcon} onClick={onClick} />,
    children: (
      <>
        <DropdownItem>text</DropdownItem>
        <DropdownItem>text</DropdownItem>
      </>
    ),
  },
}
