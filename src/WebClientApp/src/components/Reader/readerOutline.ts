export type ReaderOutlineItem = {
  id: string
  title: string
  pageNumber: number | null
  url: string | null
  children: ReaderOutlineItem[]
  depth: number
  isExternal: boolean
  isNavigable: boolean
}

export type ReaderOutlineFlatItem = {
  item: ReaderOutlineItem
  parentId: string | null
}

export const flattenReaderOutline = (
  items: ReaderOutlineItem[],
  parentId: string | null = null,
): ReaderOutlineFlatItem[] => {
  return items.flatMap(item => {
    const current: ReaderOutlineFlatItem = { item, parentId }
    const children = flattenReaderOutline(item.children, item.id)
    return [current, ...children]
  })
}

export const getOutlinePath = (parentById: Map<string, string | null>, targetId: string | null): string[] => {
  if (!targetId) {
    return []
  }

  const path: string[] = []
  let currentId: string | null = targetId

  while (currentId) {
    path.unshift(currentId)
    currentId = parentById.get(currentId) ?? null
  }

  return path
}

export const findBestActiveOutlineId = (items: ReaderOutlineFlatItem[], currentPage: number): string | null => {
  let bestItem: ReaderOutlineItem | null = null

  for (const { item } of items) {
    if (item.pageNumber === null) {
      continue
    }

    if (item.pageNumber <= currentPage) {
      if (!bestItem) {
        bestItem = item
        continue
      }

      const bestPageNumber = bestItem.pageNumber ?? -1
      if (item.pageNumber > bestPageNumber) {
        bestItem = item
        continue
      }

      if (item.pageNumber === bestPageNumber && item.depth > bestItem.depth) {
        bestItem = item
      }
    }
  }

  if (bestItem) {
    return bestItem.id
  }

  const firstNavigable = items.find(({ item }) => item.pageNumber !== null)
  return firstNavigable?.item.id ?? null
}
