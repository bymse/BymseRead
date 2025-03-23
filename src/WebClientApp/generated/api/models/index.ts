/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import {
  type AdditionalDataHolder,
  type ApiError,
  type Guid,
  type Parsable,
  type ParseNode,
  type SerializationWriter,
} from '@microsoft/kiota-abstractions'

export interface AddLastPageBookmarkRequest extends Parsable {
  /**
   * The page property
   */
  page?: number | null
}

export interface BookInfo extends Parsable {
  /**
   * The bookFile property
   */
  bookFile?: FileInfo | null
  /**
   * The bookId property
   */
  bookId?: Guid | null
  /**
   * The coverUrl property
   */
  coverUrl?: string | null
  /**
   * The currentPage property
   */
  currentPage?: number | null
  /**
   * The lastBookmark property
   */
  lastBookmark?: BookmarkInfo | null
  /**
   * The pages property
   */
  pages?: number | null
  /**
   * The title property
   */
  title?: string | null
}

export interface BookmarkInfo extends Parsable {
  /**
   * The createdAt property
   */
  createdAt?: Date | null
  /**
   * The page property
   */
  page?: number | null
}

export interface BooksCollectionInfo extends Parsable {
  /**
   * The activeBooks property
   */
  activeBooks?: BookShortInfo[] | null
  /**
   * The archivedBooks property
   */
  archivedBooks?: BookShortInfo[] | null
  /**
   * The newBooks property
   */
  newBooks?: BookShortInfo[] | null
  /**
   * The tlDrBooks property
   */
  tlDrBooks?: BookShortInfo[] | null
}

export interface BookShortInfo extends Parsable {
  /**
   * The bookId property
   */
  bookId?: Guid | null
  /**
   * The coverUrl property
   */
  coverUrl?: string | null
  /**
   * The percentageFinished property
   */
  percentageFinished?: number | null
  /**
   * The title property
   */
  title?: string | null
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {AddLastPageBookmarkRequest}
 */
// @ts-ignore
export function createAddLastPageBookmarkRequestFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoAddLastPageBookmarkRequest
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {BookInfo}
 */
// @ts-ignore
export function createBookInfoFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoBookInfo
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {BookmarkInfo}
 */
// @ts-ignore
export function createBookmarkInfoFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoBookmarkInfo
}

export interface CreateBookRequest extends Parsable {
  /**
   * The fileUploadKey property
   */
  fileUploadKey?: string | null
  /**
   * The title property
   */
  title?: string | null
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {BooksCollectionInfo}
 */
// @ts-ignore
export function createBooksCollectionInfoFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoBooksCollectionInfo
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {BookShortInfo}
 */
// @ts-ignore
export function createBookShortInfoFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoBookShortInfo
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {CreateBookRequest}
 */
// @ts-ignore
export function createCreateBookRequestFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoCreateBookRequest
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {CreatedBookResult}
 */
// @ts-ignore
export function createCreatedBookResultFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoCreatedBookResult
}

export interface CreatedBookResult extends Parsable {
  /**
   * The bookId property
   */
  bookId?: Guid | null
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {FileInfo}
 */
// @ts-ignore
export function createFileInfoFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoFileInfo
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {PreparedFileUploadResult}
 */
// @ts-ignore
export function createPreparedFileUploadResultFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoPreparedFileUploadResult
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {PrepareFileUploadRequest}
 */
// @ts-ignore
export function createPrepareFileUploadRequestFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoPrepareFileUploadRequest
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {ProblemDetails}
 */
// @ts-ignore
export function createProblemDetailsFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoProblemDetails
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {RedirectProblemDetails}
 */
// @ts-ignore
export function createRedirectProblemDetailsFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoRedirectProblemDetails
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {UpdateBookRequest}
 */
// @ts-ignore
export function createUpdateBookRequestFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoUpdateBookRequest
}

/**
 * Creates a new instance of the appropriate class based on discriminator value
 * @param parseNode The parse node to use to read the discriminator value and create the object
 * @returns {UpdateCurrentPageRequest}
 */
// @ts-ignore
export function createUpdateCurrentPageRequestFromDiscriminatorValue(
  parseNode: ParseNode | undefined,
): (instance?: Parsable) => Record<string, (node: ParseNode) => void> {
  return deserializeIntoUpdateCurrentPageRequest
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoAddLastPageBookmarkRequest(
  addLastPageBookmarkRequest: Partial<AddLastPageBookmarkRequest> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    page: n => {
      addLastPageBookmarkRequest.page = n.getNumberValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoBookInfo(
  bookInfo: Partial<BookInfo> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    bookFile: n => {
      bookInfo.bookFile = n.getObjectValue<FileInfo>(createFileInfoFromDiscriminatorValue)
    },
    bookId: n => {
      bookInfo.bookId = n.getGuidValue()
    },
    coverUrl: n => {
      bookInfo.coverUrl = n.getStringValue()
    },
    currentPage: n => {
      bookInfo.currentPage = n.getNumberValue()
    },
    lastBookmark: n => {
      bookInfo.lastBookmark = n.getObjectValue<BookmarkInfo>(createBookmarkInfoFromDiscriminatorValue)
    },
    pages: n => {
      bookInfo.pages = n.getNumberValue()
    },
    title: n => {
      bookInfo.title = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoBookmarkInfo(
  bookmarkInfo: Partial<BookmarkInfo> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    createdAt: n => {
      bookmarkInfo.createdAt = n.getDateValue()
    },
    page: n => {
      bookmarkInfo.page = n.getNumberValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoBooksCollectionInfo(
  booksCollectionInfo: Partial<BooksCollectionInfo> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    activeBooks: n => {
      booksCollectionInfo.activeBooks = n.getCollectionOfObjectValues<BookShortInfo>(
        createBookShortInfoFromDiscriminatorValue,
      )
    },
    archivedBooks: n => {
      booksCollectionInfo.archivedBooks = n.getCollectionOfObjectValues<BookShortInfo>(
        createBookShortInfoFromDiscriminatorValue,
      )
    },
    newBooks: n => {
      booksCollectionInfo.newBooks = n.getCollectionOfObjectValues<BookShortInfo>(
        createBookShortInfoFromDiscriminatorValue,
      )
    },
    tlDrBooks: n => {
      booksCollectionInfo.tlDrBooks = n.getCollectionOfObjectValues<BookShortInfo>(
        createBookShortInfoFromDiscriminatorValue,
      )
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoBookShortInfo(
  bookShortInfo: Partial<BookShortInfo> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    bookId: n => {
      bookShortInfo.bookId = n.getGuidValue()
    },
    coverUrl: n => {
      bookShortInfo.coverUrl = n.getStringValue()
    },
    percentageFinished: n => {
      bookShortInfo.percentageFinished = n.getNumberValue()
    },
    title: n => {
      bookShortInfo.title = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoCreateBookRequest(
  createBookRequest: Partial<CreateBookRequest> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    fileUploadKey: n => {
      createBookRequest.fileUploadKey = n.getStringValue()
    },
    title: n => {
      createBookRequest.title = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoCreatedBookResult(
  createdBookResult: Partial<CreatedBookResult> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    bookId: n => {
      createdBookResult.bookId = n.getGuidValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoFileInfo(
  fileInfo: Partial<FileInfo> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    fileUrl: n => {
      fileInfo.fileUrl = n.getStringValue()
    },
    name: n => {
      fileInfo.name = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoPreparedFileUploadResult(
  preparedFileUploadResult: Partial<PreparedFileUploadResult> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    fileUploadKey: n => {
      preparedFileUploadResult.fileUploadKey = n.getStringValue()
    },
    uploadUrl: n => {
      preparedFileUploadResult.uploadUrl = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoPrepareFileUploadRequest(
  prepareFileUploadRequest: Partial<PrepareFileUploadRequest> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    fileName: n => {
      prepareFileUploadRequest.fileName = n.getStringValue()
    },
    fileSize: n => {
      prepareFileUploadRequest.fileSize = n.getNumberValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoProblemDetails(
  problemDetails: Partial<ProblemDetails> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    detail: n => {
      problemDetails.detail = n.getStringValue()
    },
    instance: n => {
      problemDetails.instance = n.getStringValue()
    },
    status: n => {
      problemDetails.status = n.getNumberValue()
    },
    title: n => {
      problemDetails.title = n.getStringValue()
    },
    type: n => {
      problemDetails.type = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoRedirectProblemDetails(
  redirectProblemDetails: Partial<RedirectProblemDetails> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    detail: n => {
      redirectProblemDetails.detail = n.getStringValue()
    },
    instance: n => {
      redirectProblemDetails.instance = n.getStringValue()
    },
    redirectUrl: n => {
      redirectProblemDetails.redirectUrl = n.getStringValue()
    },
    status: n => {
      redirectProblemDetails.status = n.getNumberValue()
    },
    title: n => {
      redirectProblemDetails.title = n.getStringValue()
    },
    type: n => {
      redirectProblemDetails.type = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoUpdateBookRequest(
  updateBookRequest: Partial<UpdateBookRequest> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    removeCover: n => {
      updateBookRequest.removeCover = n.getBooleanValue()
    },
    title: n => {
      updateBookRequest.title = n.getStringValue()
    },
    uploadedBookFileKey: n => {
      updateBookRequest.uploadedBookFileKey = n.getStringValue()
    },
    uploadedCoverFileKey: n => {
      updateBookRequest.uploadedCoverFileKey = n.getStringValue()
    },
  }
}

/**
 * The deserialization information for the current model
 * @returns {Record<string, (node: ParseNode) => void>}
 */
// @ts-ignore
export function deserializeIntoUpdateCurrentPageRequest(
  updateCurrentPageRequest: Partial<UpdateCurrentPageRequest> | undefined = {},
): Record<string, (node: ParseNode) => void> {
  return {
    page: n => {
      updateCurrentPageRequest.page = n.getNumberValue()
    },
  }
}

export interface FileInfo extends Parsable {
  /**
   * The fileUrl property
   */
  fileUrl?: string | null
  /**
   * The name property
   */
  name?: string | null
}

export interface PreparedFileUploadResult extends Parsable {
  /**
   * The fileUploadKey property
   */
  fileUploadKey?: string | null
  /**
   * The uploadUrl property
   */
  uploadUrl?: string | null
}

export interface PrepareFileUploadRequest extends Parsable {
  /**
   * The fileName property
   */
  fileName?: string | null
  /**
   * The fileSize property
   */
  fileSize?: number | null
}

export interface ProblemDetails extends AdditionalDataHolder, ApiError, Parsable {
  /**
   * Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.
   */
  additionalData?: Record<string, unknown>
  /**
   * The detail property
   */
  detail?: string | null
  /**
   * The instance property
   */
  instance?: string | null
  /**
   * The status property
   */
  status?: number | null
  /**
   * The title property
   */
  title?: string | null
  /**
   * The type property
   */
  type?: string | null
}

export interface RedirectProblemDetails extends AdditionalDataHolder, ApiError, Parsable {
  /**
   * Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.
   */
  additionalData?: Record<string, unknown>
  /**
   * The detail property
   */
  detail?: string | null
  /**
   * The instance property
   */
  instance?: string | null
  /**
   * The redirectUrl property
   */
  redirectUrl?: string | null
  /**
   * The status property
   */
  status?: number | null
  /**
   * The title property
   */
  title?: string | null
  /**
   * The type property
   */
  type?: string | null
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeAddLastPageBookmarkRequest(
  writer: SerializationWriter,
  addLastPageBookmarkRequest: Partial<AddLastPageBookmarkRequest> | undefined | null = {},
): void {
  if (addLastPageBookmarkRequest) {
    writer.writeNumberValue('page', addLastPageBookmarkRequest.page)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeBookInfo(
  writer: SerializationWriter,
  bookInfo: Partial<BookInfo> | undefined | null = {},
): void {
  if (bookInfo) {
    writer.writeObjectValue<FileInfo>('bookFile', bookInfo.bookFile, serializeFileInfo)
    writer.writeGuidValue('bookId', bookInfo.bookId)
    writer.writeStringValue('coverUrl', bookInfo.coverUrl)
    writer.writeNumberValue('currentPage', bookInfo.currentPage)
    writer.writeObjectValue<BookmarkInfo>('lastBookmark', bookInfo.lastBookmark, serializeBookmarkInfo)
    writer.writeNumberValue('pages', bookInfo.pages)
    writer.writeStringValue('title', bookInfo.title)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeBookmarkInfo(
  writer: SerializationWriter,
  bookmarkInfo: Partial<BookmarkInfo> | undefined | null = {},
): void {
  if (bookmarkInfo) {
    writer.writeDateValue('createdAt', bookmarkInfo.createdAt)
    writer.writeNumberValue('page', bookmarkInfo.page)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeBooksCollectionInfo(
  writer: SerializationWriter,
  booksCollectionInfo: Partial<BooksCollectionInfo> | undefined | null = {},
): void {
  if (booksCollectionInfo) {
    writer.writeCollectionOfObjectValues<BookShortInfo>(
      'activeBooks',
      booksCollectionInfo.activeBooks,
      serializeBookShortInfo,
    )
    writer.writeCollectionOfObjectValues<BookShortInfo>(
      'archivedBooks',
      booksCollectionInfo.archivedBooks,
      serializeBookShortInfo,
    )
    writer.writeCollectionOfObjectValues<BookShortInfo>(
      'newBooks',
      booksCollectionInfo.newBooks,
      serializeBookShortInfo,
    )
    writer.writeCollectionOfObjectValues<BookShortInfo>(
      'tlDrBooks',
      booksCollectionInfo.tlDrBooks,
      serializeBookShortInfo,
    )
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeBookShortInfo(
  writer: SerializationWriter,
  bookShortInfo: Partial<BookShortInfo> | undefined | null = {},
): void {
  if (bookShortInfo) {
    writer.writeGuidValue('bookId', bookShortInfo.bookId)
    writer.writeStringValue('coverUrl', bookShortInfo.coverUrl)
    writer.writeNumberValue('percentageFinished', bookShortInfo.percentageFinished)
    writer.writeStringValue('title', bookShortInfo.title)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeCreateBookRequest(
  writer: SerializationWriter,
  createBookRequest: Partial<CreateBookRequest> | undefined | null = {},
): void {
  if (createBookRequest) {
    writer.writeStringValue('fileUploadKey', createBookRequest.fileUploadKey)
    writer.writeStringValue('title', createBookRequest.title)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeCreatedBookResult(
  writer: SerializationWriter,
  createdBookResult: Partial<CreatedBookResult> | undefined | null = {},
): void {
  if (createdBookResult) {
    writer.writeGuidValue('bookId', createdBookResult.bookId)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeFileInfo(
  writer: SerializationWriter,
  fileInfo: Partial<FileInfo> | undefined | null = {},
): void {
  if (fileInfo) {
    writer.writeStringValue('fileUrl', fileInfo.fileUrl)
    writer.writeStringValue('name', fileInfo.name)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializePreparedFileUploadResult(
  writer: SerializationWriter,
  preparedFileUploadResult: Partial<PreparedFileUploadResult> | undefined | null = {},
): void {
  if (preparedFileUploadResult) {
    writer.writeStringValue('fileUploadKey', preparedFileUploadResult.fileUploadKey)
    writer.writeStringValue('uploadUrl', preparedFileUploadResult.uploadUrl)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializePrepareFileUploadRequest(
  writer: SerializationWriter,
  prepareFileUploadRequest: Partial<PrepareFileUploadRequest> | undefined | null = {},
): void {
  if (prepareFileUploadRequest) {
    writer.writeStringValue('fileName', prepareFileUploadRequest.fileName)
    writer.writeNumberValue('fileSize', prepareFileUploadRequest.fileSize)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeProblemDetails(
  writer: SerializationWriter,
  problemDetails: Partial<ProblemDetails> | undefined | null = {},
): void {
  if (problemDetails) {
    writer.writeStringValue('detail', problemDetails.detail)
    writer.writeStringValue('instance', problemDetails.instance)
    writer.writeNumberValue('status', problemDetails.status)
    writer.writeStringValue('title', problemDetails.title)
    writer.writeStringValue('type', problemDetails.type)
    writer.writeAdditionalData(problemDetails.additionalData)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeRedirectProblemDetails(
  writer: SerializationWriter,
  redirectProblemDetails: Partial<RedirectProblemDetails> | undefined | null = {},
): void {
  if (redirectProblemDetails) {
    writer.writeStringValue('detail', redirectProblemDetails.detail)
    writer.writeStringValue('instance', redirectProblemDetails.instance)
    writer.writeStringValue('redirectUrl', redirectProblemDetails.redirectUrl)
    writer.writeNumberValue('status', redirectProblemDetails.status)
    writer.writeStringValue('title', redirectProblemDetails.title)
    writer.writeStringValue('type', redirectProblemDetails.type)
    writer.writeAdditionalData(redirectProblemDetails.additionalData)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeUpdateBookRequest(
  writer: SerializationWriter,
  updateBookRequest: Partial<UpdateBookRequest> | undefined | null = {},
): void {
  if (updateBookRequest) {
    writer.writeBooleanValue('removeCover', updateBookRequest.removeCover)
    writer.writeStringValue('title', updateBookRequest.title)
    writer.writeStringValue('uploadedBookFileKey', updateBookRequest.uploadedBookFileKey)
    writer.writeStringValue('uploadedCoverFileKey', updateBookRequest.uploadedCoverFileKey)
  }
}

/**
 * Serializes information the current object
 * @param writer Serialization writer to use to serialize this model
 */
// @ts-ignore
export function serializeUpdateCurrentPageRequest(
  writer: SerializationWriter,
  updateCurrentPageRequest: Partial<UpdateCurrentPageRequest> | undefined | null = {},
): void {
  if (updateCurrentPageRequest) {
    writer.writeNumberValue('page', updateCurrentPageRequest.page)
  }
}

export interface UpdateBookRequest extends Parsable {
  /**
   * The removeCover property
   */
  removeCover?: boolean | null
  /**
   * The title property
   */
  title?: string | null
  /**
   * The uploadedBookFileKey property
   */
  uploadedBookFileKey?: string | null
  /**
   * The uploadedCoverFileKey property
   */
  uploadedCoverFileKey?: string | null
}

export interface UpdateCurrentPageRequest extends Parsable {
  /**
   * The page property
   */
  page?: number | null
}

/* tslint:enable */
/* eslint-enable */
