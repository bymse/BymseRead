import { Queue } from 'workbox-background-sync'

const bookQueues = new Map<string, Queue>()

const getQueueForBook = (bookId: string): Queue => {
  let queue = bookQueues.get(bookId)
  if (!queue) {
    queue = new Queue(`book-${bookId}-queue`, {
      onSync: async ({ queue: q }) => {
        let entry = await q.shiftRequest()
        while (entry) {
          try {
            await fetch(entry.request)
          } catch (error) {
            await q.unshiftRequest(entry)
            throw error
          }
          entry = await q.shiftRequest()
        }
      },
    })
    bookQueues.set(bookId, queue)
  }
  return queue
}

export const initPostponedRequests = () => {}
