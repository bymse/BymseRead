import { useEffect } from 'preact/hooks'
import { isPwa } from '@utils/isPwa.ts'

export const usePageTitle = (title?: string | null) => {
  useEffect(() => {
    const suffix = isPwa() ? '' : ' — BymseRead'
    if (title) {
      document.title = `${title}${suffix}`
    } else {
      const url = window.location.hostname + window.location.pathname
      document.title = `${url}${suffix}`
    }
  }, [title])
}
