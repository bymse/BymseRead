import type { BookCollectionItem, BookInfo } from '@api/models'
import { BookMeta } from '@storage/db.ts'
import { BookFiles } from '@storage/filesCacheMessages.ts'

export interface EvalResult {
  newBook?: BookMeta
  newFiles?: BookFiles
  oldFiles?: BookFiles
}

export const evaluateForStorageUpdate = (
  activeBook: BookInfo | BookCollectionItem,
  storedBook?: BookMeta,
): EvalResult => {
  if (!storedBook) {
    return {
      newBook: mapWithFiles(activeBook, undefined, undefined),
      newFiles: {
        bookId: activeBook.bookId!,
        fileUrl: getFileUrl(activeBook),
        coverUrl: activeBook.coverUrl ?? undefined,
      },
    }
  }

  let bookToAdd: BookMeta | undefined = undefined
  let filesToAdd: BookFiles | undefined = undefined
  let filesToRemove: BookFiles | undefined = undefined

  const isSameFile = isSameUrl(storedBook.fileUrl, getFileUrl(activeBook))
  const isSameCover = isSameUrl(storedBook.coverUrl, activeBook.coverUrl)

  if (!isSameFile || !isSameCover) {
    filesToRemove = {
      bookId: activeBook.bookId!,
      fileUrl: isSameFile ? undefined : (storedBook.fileUrl ?? undefined),
      coverUrl: isSameCover ? undefined : (storedBook.coverUrl ?? undefined),
    }
    filesToAdd = {
      bookId: activeBook.bookId!,
      fileUrl: isSameFile ? undefined : getFileUrl(activeBook),
      coverUrl: isSameCover ? undefined : (activeBook.coverUrl ?? undefined),
    }
  }

  if (
    storedBook.currentPage?.page !== activeBook.currentPage ||
    storedBook.lastBookmark?.page !== activeBook.lastBookmark?.page ||
    storedBook.title !== activeBook.title ||
    storedBook.pages !== activeBook.pages
  ) {
    bookToAdd = mapWithFiles(activeBook, storedBook.fileUrl, storedBook.coverUrl)
  }

  return { newBook: bookToAdd, newFiles: filesToAdd, oldFiles: filesToRemove }
}

export const getFileUrl = (book: BookInfo | BookCollectionItem): string | undefined => {
  if ('fileUrl' in book) {
    return book.fileUrl ?? undefined
  }

  if ('bookFile' in book) {
    return book.bookFile!.fileUrl ?? undefined
  }

  return undefined
}

export const isSameUrl = (url1?: string | null, url2?: string | null): boolean => {
  try {
    const url1Url = new URL(url1!)
    const url2Url = new URL(url2!)
    return url1Url.origin === url2Url.origin && url1Url.pathname === url2Url.pathname
  } catch {
    return false
  }
}

export const mapWithFiles = (book: BookInfo | BookCollectionItem, fileUrl?: string, coverUrl?: string): BookMeta => {
  return {
    fileUrl: fileUrl,
    coverUrl: coverUrl,
    bookId: book.bookId!,
    pages: book.pages ?? undefined,
    title: book.title!,
    lastBookmark: book.lastBookmark
      ? {
          page: book.lastBookmark.page!,
          createdAt: book.lastBookmark.createdAt!,
        }
      : undefined,
    currentPage: book.currentPage ? { page: book.currentPage } : undefined,
  }
}
