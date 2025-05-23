/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { LastPageRequestBuilderRequestsMetadata, type LastPageRequestBuilder } from './lastPage/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /web-api/books/{bookId}/bookmarks
 */
export interface BookmarksRequestBuilder extends BaseRequestBuilder<BookmarksRequestBuilder> {
    /**
     * The lastPage property
     */
    get lastPage(): LastPageRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const BookmarksRequestBuilderUriTemplate = "{+baseurl}/web-api/books/{bookId}/bookmarks";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const BookmarksRequestBuilderNavigationMetadata: Record<Exclude<keyof BookmarksRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    lastPage: {
        requestsMetadata: LastPageRequestBuilderRequestsMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
