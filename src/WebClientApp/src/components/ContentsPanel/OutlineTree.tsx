import styles from './ContentsPanel.module.scss'
import { ReaderOutlineItem } from '@components/Reader/readerOutline.ts'
import { ChevronIcon } from '@icons/ChevronIcon.tsx'
import { OutlineLink } from './OutlineLink.tsx'
import cn from 'classnames'

type OutlineTreeContext = {
  activeItemId: string | null
  expandedIds: Set<string>
  activePathIds: Set<string>
  onToggleExpand: (id: string) => void
  onNavigate: (item: ReaderOutlineItem) => void
}

export type OutlineTreeProps = OutlineTreeContext & {
  items: ReaderOutlineItem[]
}

type OutlineRowProps = OutlineTreeContext & {
  item: ReaderOutlineItem
}

const OutlineRow = ({ item, ...context }: OutlineRowProps) => {
  const { activeItemId, expandedIds, activePathIds, onToggleExpand, onNavigate } = context
  const hasChildren = item.children.length > 0
  const isExpanded = expandedIds.has(item.id)
  const isActive = item.id === activeItemId
  const isActivePath = activePathIds.has(item.id)
  const isDisabled = !item.isNavigable

  return (
    <>
      <div
        className={cn(styles.row, {
          [styles.active]: isActive,
          [styles.disabled]: isDisabled,
          [styles.withGuide]: item.depth > 0,
          [styles.activePath]: isActivePath && !isActive,
        })}
        style={{ ['--outline-depth' as string]: `${item.depth}` }}
        data-testid="contents-outline-row"
        data-outline-id={item.id}
        data-active={isActive ? 'true' : 'false'}
      >
        <div className={styles.rowContent} style={{ paddingLeft: `${item.depth * 16}px` }}>
          <div className={styles.chevronSlot}>
            {hasChildren && (
              <button
                type="button"
                className={styles.chevronButton}
                data-testid="contents-outline-chevron"
                data-outline-id={item.id}
                onClick={() => onToggleExpand(item.id)}
                aria-label={isExpanded ? 'Collapse section' : 'Expand section'}
              >
                <ChevronIcon className={cn(styles.chevronIcon, { [styles.expanded]: isExpanded })} />
              </button>
            )}
          </div>
          <OutlineLink item={item} onNavigate={onNavigate} />
        </div>
      </div>
      {hasChildren && isExpanded && item.children.map(child => <OutlineRow key={child.id} item={child} {...context} />)}
    </>
  )
}

export const OutlineTree = ({ items, ...context }: OutlineTreeProps) => (
  <>
    {items.map(item => (
      <OutlineRow key={item.id} item={item} {...context} />
    ))}
  </>
)
