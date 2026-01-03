import { ProblemDetails } from '@api/models'
import { bymseReadClient } from '@hooks/useWebApiClient.ts'
import { ApiError } from '@microsoft/kiota-abstractions'
import { getNextPostponedUpdate, removePostponedUpdate } from '@storage/postponedUpdatesStore.ts'

declare let self: ServiceWorkerGlobalScope

interface SyncEvent extends ExtendableEvent {
  tag: string
  lastChance: boolean
}

const TAG = 'bymse-read-postponed'

export const handlePostponedUpdates = () => {
  if ('sync' in self.registration) {
    // @ts-expect-error sync is unknown
    self.addEventListener('sync', (event: SyncEvent) => {
      if (event.tag === TAG) {
        const syncComplete = async () => {
          await handleSync()
        }
        event.waitUntil(syncComplete())
      }
    })
  } else {
    void handleSync()
  }
}

const handleSync = async (): Promise<void> => {
  while (true) {
    const nextUpdate = await getNextPostponedUpdate()
    if (!nextUpdate) {
      return
    }

    try {
      if (nextUpdate.currentPage) {
        try {
          await bymseReadClient.webApi.books.byBookId(nextUpdate.bookId).progress.currentPage.put({
            page: nextUpdate.currentPage.page,
            changedAt: nextUpdate.currentPage.createdAt,
          })
        } catch (e) {
          handleError(e)
        }
      }

      if (nextUpdate.lastBookmark) {
        try {
          await bymseReadClient.webApi.books.byBookId(nextUpdate.bookId).bookmarks.lastPage.post({
            page: nextUpdate.lastBookmark.page,
            changedAt: nextUpdate.lastBookmark.createdAt,
          })
        } catch (e) {
          handleError(e)
        }
      }
    } catch {
      const dateStr = nextUpdate.currentPage?.createdAt ?? nextUpdate.lastBookmark?.createdAt
      if (!dateStr) {
        await removePostponedUpdate(nextUpdate.bookId)
        continue
      }

      const date = new Date(dateStr)
      const fiveDaysAgo = new Date(Date.now() - 5 * 24 * 60 * 60 * 1000)
      if (date > fiveDaysAgo) {
        await removePostponedUpdate(nextUpdate.bookId)
      }
    }
  }
}

const handleError = (error: unknown) => {
  const typedError = error as ApiError | ProblemDetails | Error
  if (!('responseStatusCode' in typedError)) {
    throw error
  }

  if (
    typedError.responseStatusCode &&
    (typedError.responseStatusCode === 401 || typedError.responseStatusCode >= 500)
  ) {
    return
  }

  throw error
}
