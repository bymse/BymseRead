import { ProblemDetails } from '@api/models'
import { bymseReadClient } from '@hooks/useWebApiClient.ts'
import { ApiError } from '@microsoft/kiota-abstractions'
import { POSTPONED_UPDATE_TAG } from '@storage/serviceWorkerMessages.ts'
import { getNextPostponedUpdate, removePostponedUpdate } from '@storage/postponedUpdatesStore.ts'

declare let self: ServiceWorkerGlobalScope

export const handlePostponedUpdates = () => {
  if ('sync' in self.registration) {
    self.addEventListener('sync', event => {
      if (event.tag === POSTPONED_UPDATE_TAG) {
        const syncComplete = async () => {
          await handleSync()
        }
        event.waitUntil(syncComplete())
      }
    })
  }
}

export const forceSync = async (): Promise<void> => {
  await handleSync()
}

const handleSync = async (): Promise<void> => {
  while (true) {
    const nextUpdate = await getNextPostponedUpdate()
    if (!nextUpdate) {
      return
    }

    const promises = [
      async () => {
        if (nextUpdate.currentPage) {
          await bymseReadClient.webApi.books.byBookId(nextUpdate.bookId).progress.currentPage.put({
            page: nextUpdate.currentPage.page,
            createdAt: nextUpdate.currentPage.createdAt,
          })
        }
      },
      async () => {
        if (nextUpdate.lastBookmark) {
          await bymseReadClient.webApi.books.byBookId(nextUpdate.bookId).bookmarks.lastPage.post({
            page: nextUpdate.lastBookmark.page,
            createdAt: nextUpdate.lastBookmark.createdAt,
          })
        }
      },
    ]

    try {
      await Promise.all(promises.map(e => e()))
    } catch (e) {
      if (shouldRetryLater(e)) {
        return
      }
    }
    await removePostponedUpdate(nextUpdate.bookId)
  }
}

const shouldRetryLater = (error: unknown) => {
  const typedError = error as ApiError | ProblemDetails | Error
  if (!('responseStatusCode' in typedError)) {
    return true
  }

  return (
    typedError.responseStatusCode! > 500 ||
    typedError.responseStatusCode === 429 ||
    typedError.responseStatusCode === 401
  )
}
