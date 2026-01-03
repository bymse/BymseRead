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

export interface ForceSync {
  type: 'FORCE_SYNC'
}

export const POSTPONED_UPDATE_TAG = 'bymse-read-postponed'

export type ServiceWorkerMessage = CacheAddFilesMessage | CacheRemoveFilesMessage | ForceSync

const waitForServiceWorker = async (): Promise<ServiceWorkerRegistration> => {
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
          resolve(registration)
        } else {
          reject(new Error('Service Worker not active'))
        }
      })
      .catch(reject)
  })
}

const postMessageToServiceWorker = async (message: ServiceWorkerMessage): Promise<void> => {
  try {
    const registration = await waitForServiceWorker()
    registration.active!.postMessage(message)
  } catch (e) {
    // eslint-disable-next-line no-console
    console.error('Failed to postMessage for cache', e, message)
  }
}

export const cacheBookFiles = async (files: BookFiles[]): Promise<void> => {
  if (files.length === 0) return

  await postMessageToServiceWorker({
    type: 'CACHE_ADD_FILES',
    payload: { files },
  })
}

export const removeBookFilesCache = async (files: BookFiles[]): Promise<void> => {
  if (files.length === 0) return

  await postMessageToServiceWorker({
    type: 'CACHE_REMOVE_FILES',
    payload: { files },
  })
}

export const forceSync = async (): Promise<void> => {
  await postMessageToServiceWorker({
    type: 'FORCE_SYNC',
  })
}

export const postponeUpdate = async (): Promise<void> => {
  try {
    const registration = await waitForServiceWorker()
    await registration.sync.register(POSTPONED_UPDATE_TAG)
  } catch (e) {
    // eslint-disable-next-line no-console
    console.error('Failed to register sync', e)
  }
}
