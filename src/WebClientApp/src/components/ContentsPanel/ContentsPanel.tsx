import styles from './ContentsPanel.module.scss'
import {
  ReaderOutlineItem,
  flattenReaderOutline,
  findBestActiveOutlineId,
  getOutlinePath,
} from '@components/Reader/readerOutline.ts'
import { RemoveIcon } from '@icons/RemoveIcon.tsx'
import { Button } from '@components/Button/Button.tsx'
import { useEffect, useMemo, useRef, useState } from 'preact/hooks'
import { OutlineTree } from './OutlineTree.tsx'

const MOBILE_MEDIA_QUERY = '(max-width: 414px)'

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

    if (navigated && window.matchMedia(MOBILE_MEDIA_QUERY).matches) {
      onClose()
    }
  }

  return (
    <aside className={styles.container} data-testid="contents-panel" ref={rootRef}>
      <h2 className={styles.heading}>Contents</h2>
      <div className={styles.close}>
        <Button icon={RemoveIcon} onClick={onClose} appearance="flat" data-testid="contents-close-button" />
      </div>
      <div className={styles.rows}>
        <OutlineTree
          items={items}
          activeItemId={activeItemId}
          expandedIds={expandedIds}
          activePathIds={activePathIds}
          onToggleExpand={handleToggleExpand}
          onNavigate={handleNavigate}
        />
      </div>
    </aside>
  )
}
