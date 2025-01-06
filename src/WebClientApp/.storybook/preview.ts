import type {Preview} from "@storybook/preact";
import {ViewportMap} from "@storybook/addon-viewport";

const viewports: ViewportMap = {
  mobile: {
    type: "mobile",
    name: "Galaxy Z Fold 6. Cover",
    styles: {
      width: "412px",
      height: "917px",
    }
  },
  openedMobile: {
    type: "tablet",
    name: "Galaxy Z Fold 6. Main",
    styles: {
      width: "700px",
      height: "840px",
    }
  },
  desktop: {
    type: "desktop",
    name: "Desktop",
    styles: {
      width: "1440px",
      height: "1024px",
    }
  }
}

const preview: Preview = {
  parameters: {
    viewport: {
      viewports,
      defaultViewport: "desktop",
    },
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },
  },
};

export default preview;
