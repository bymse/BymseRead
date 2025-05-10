import { useEffect } from 'preact/hooks'
import { isPwa } from '@utils/isPwa.ts'

export const usePageTitle = (title?: string | null) => {
  useEffect(() => {
    const suffix = isPwa() ? '' : ' — BymseRead'
    if (title) {
      document.title = `${title}${suffix}`
    } else {
      document.title = ''
    }
  }, [title])
}
