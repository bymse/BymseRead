export {}

declare global {
  interface SyncManager {
    getTags(): Promise<string[]>
    register(tag: string): Promise<void>
  }

  interface ServiceWorkerRegistration {
    readonly sync: SyncManager
  }

  interface SyncEvent extends ExtendableEvent {
    readonly lastChance: boolean
    readonly tag: string
  }

  interface ServiceWorkerGlobalScopeEventMap {
    sync: SyncEvent
  }
}
