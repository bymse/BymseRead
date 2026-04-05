import styles from './ContentsPanel.module.scss'
import { ReaderOutlineItem } from '@components/Reader/readerOutline.ts'
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
  const isInternal = item.pageNumber !== null
  const isExternal = item.isExternal && item.url !== null
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
                <ChevronIcon expanded={isExpanded} />
              </button>
            )}
          </div>
          {isInternal && (
            <button
              type="button"
              className={styles.linkArea}
              data-testid="contents-outline-link"
              data-outline-id={item.id}
              onClick={() => onNavigate(item)}
            >
              <span className={styles.rowTitle}>{item.title}</span>
              <span className={styles.dots}></span>
              <span className={styles.pageNumber}>{item.pageNumber}</span>
            </button>
          )}
          {isExternal && (
            <a
              href={item.url ?? undefined}
              target="_blank"
              rel="noopener noreferrer"
              className={styles.linkArea}
              data-testid="contents-outline-link"
              data-outline-id={item.id}
              onClick={() => onNavigate(item)}
            >
              <span className={styles.rowTitle}>{item.title}</span>
            </a>
          )}
          {!isInternal && !isExternal && (
            <div className={styles.linkArea}>
              <span className={styles.rowTitle}>{item.title}</span>
            </div>
          )}
        </div>
      </div>
      {hasChildren && isExpanded && item.children.map(child => <OutlineRow key={child.id} item={child} {...context} />)}
    </>
  )
}

const ChevronIcon = ({ expanded }: { expanded: boolean }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    width="24"
    height="24"
    viewBox="0 0 24 24"
    fill="none"
    className={cn(styles.chevronIcon, { [styles.expanded]: expanded })}
  >
    <path
      fillRule="evenodd"
      clipRule="evenodd"
      d="M8.29289 5.29289C8.68342 4.90237 9.31658 4.90237 9.70711 5.29289L15.7071 11.2929C16.0976 11.6834 16.0976 12.3166 15.7071 12.7071L9.70711 18.7071C9.31658 19.0976 8.68342 19.0976 8.29289 18.7071C7.90237 18.3166 7.90237 17.6834 8.29289 17.2929L13.5858 12L8.29289 6.70711C7.90237 6.31658 7.90237 5.68342 8.29289 5.29289Z"
      fill="currentColor"
    />
  </svg>
)

export const OutlineTree = ({ items, ...context }: OutlineTreeProps) => (
  <>
    {items.map(item => (
      <OutlineRow key={item.id} item={item} {...context} />
    ))}
  </>
)
