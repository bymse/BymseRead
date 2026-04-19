import styles from './ContentsPanel.module.scss'
import {
  ReaderOutlineItem,
  flattenReaderOutline,
  findBestActiveOutlineId,
  getOutlinePath,
} from '@components/Reader/readerOutline.ts'
import { SidePanel } from '@components/SidePanel/SidePanel.tsx'
import { useEffect, useMemo, useRef, useState } from 'preact/hooks'
import { OutlineTree } from './OutlineTree.tsx'

const TABLET_MEDIA_QUERY = '(max-width: 820px)'

export type ContentsPanelProps = {
  items: ReaderOutlineItem[]
  currentPage: number
  onNavigate: (page: number) => void
  onClose: () => void
}

export const ContentsPanel = ({ items, currentPage, onNavigate, onClose }: ContentsPanelProps) => {
  const rootRef = useRef<HTMLDivElement>(null)

  const flatItems = useMemo(() => flattenReaderOutline(items), [items])
  const parentById = useMemo(() => new Map(flatItems.map(({ item, parentId }) => [item.id, parentId])), [flatItems])
  const activeItemId = useMemo(() => {
    if (!items.length) {
      return null
    }
    return findBestActiveOutlineId(flatItems, currentPage)
  }, [currentPage, flatItems, items.length])
  const activePath = useMemo(() => getOutlinePath(parentById, activeItemId), [activeItemId, parentById])
  const activePathIds = useMemo(() => new Set(activePath), [activePath])

  const [expandedIds, setExpandedIds] = useState<Set<string>>(() => new Set(activePath))

  useEffect(() => {
    if (!activeItemId || !rootRef.current) {
      return
    }

    const activeRow = rootRef.current.querySelector<HTMLElement>(`[data-outline-id="${activeItemId}"]`)
    activeRow?.scrollIntoView({ block: 'nearest' })
  }, [activeItemId, expandedIds])

  const handleToggleExpand = (id: string) => {
    setExpandedIds(current => {
      const next = new Set(current)
      if (next.has(id)) {
        next.delete(id)
      } else {
        next.add(id)
      }
      return next
    })
  }

  const handleNavigate = (item: ReaderOutlineItem) => {
    const navigated = item.pageNumber !== null || item.isExternal

    if (item.pageNumber !== null) {
      onNavigate(item.pageNumber)
    }

    if (navigated && window.matchMedia(TABLET_MEDIA_QUERY).matches) {
      onClose()
    }
  }

  return (
    <SidePanel
      title="Contents"
      onClose={onClose}
      fullHeight
      className={styles.container}
      testId="contents-panel"
      closeButtonTestId="contents-close-button"
    >
      <div className={styles.rows} ref={rootRef}>
        <OutlineTree
          items={items}
          activeItemId={activeItemId}
          expandedIds={expandedIds}
          activePathIds={activePathIds}
          onToggleExpand={handleToggleExpand}
          onNavigate={handleNavigate}
        />
      </div>
    </SidePanel>
  )
}
