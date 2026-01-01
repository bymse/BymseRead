export interface BookFiles {
  bookId: string
  fileUrl?: string
  coverUrl?: string
}

export interface CacheAddFilesMessage {
  type: 'CACHE_ADD_FILES'
  payload: {
    files: Array<BookFiles>
  }
}

export interface CacheRemoveFilesMessage {
  type: 'CACHE_REMOVE_FILES'
  payload: {
    files: Array<BookFiles>
  }
}

export type ServiceWorkerMessage = CacheAddFilesMessage | CacheRemoveFilesMessage

const waitForServiceWorker = async (): Promise<ServiceWorker> => {
  if (!('serviceWorker' in navigator)) {
    throw new Error('Service Worker not supported')
  }

  return new Promise((resolve, reject) => {
    const timeout = setTimeout(() => {
      reject(new Error('Service Worker registration timeout'))
    }, 5000)

    navigator.serviceWorker.ready
      .then(registration => {
        clearTimeout(timeout)
        if (registration.active) {
          resolve(registration.active)
        } else {
          reject(new Error('Service Worker not active'))
        }
      })
      .catch(reject)
  })
}

const postMessageToServiceWorker = async (message: ServiceWorkerMessage): Promise<void> => {
  try {
    const sw = await waitForServiceWorker()
    sw.postMessage(message)
  } catch (e) {
    // eslint-disable-next-line no-console
    console.error('Failed to postMessage for cache', e, message)
  }
}

export const cacheBookFiles = async (files: BookFiles[]): Promise<void> => {
  if (!files || files.length === 0) {
    return
  }

  await postMessageToServiceWorker({
    type: 'CACHE_ADD_FILES',
    payload: { files },
  })
}

export const removeBookFilesCache = async (files: BookFiles[]): Promise<void> => {
  if (!files || files.length === 0) {
    return
  }

  await postMessageToServiceWorker({
    type: 'CACHE_REMOVE_FILES',
    payload: { files },
  })
}
