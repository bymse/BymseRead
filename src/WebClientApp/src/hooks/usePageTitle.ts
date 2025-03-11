import { useEffect } from 'preact/hooks'

export const usePageTitle = (title?: string | null) => {
  useEffect(() => {
    if (title) {
      document.title = `${title} — BymseRead`
    } else {
      document.title = 'BymseRead'
    }
  }, [title])
}
