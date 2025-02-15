import type { Preview } from '@storybook/preact'
import { ViewportMap } from '@storybook/addon-viewport'
import '../src/global.css'

const viewports: ViewportMap = {
  coverMobile: {
    type: 'mobile',
    name: 'Mobile',
    styles: {
      width: '413px',
      height: '917px',
    },
  },
  mainMobile: {
    type: 'tablet',
    name: 'Tablet',
    styles: {
      width: '819px',
      height: '840px',
    },
  },
  desktop: {
    type: 'desktop',
    name: 'Desktop',
    styles: {
      width: '1440px',
      height: '1024px',
    },
  },
}

const preview: Preview = {
  parameters: {
    viewport: {
      viewports,
      defaultViewport: 'desktop',
    },
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },
  },
}

export default preview
