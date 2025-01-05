import {Button} from './Button';
import {Meta, StoryObj} from "@storybook/preact";

const meta: Meta<typeof Button> = {
    component: Button,
    args: {
        label: 'Button',
    }
};
export default meta;

type Story = StoryObj<typeof Button>;

export const Primary: Story = {
    args: {
        label: 'Button',
    },
};