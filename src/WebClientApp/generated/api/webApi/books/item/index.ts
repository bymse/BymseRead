/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createBookInfoFromDiscriminatorValue, createProblemDetailsFromDiscriminatorValue, createRedirectProblemDetailsFromDiscriminatorValue, type BookInfo, type ProblemDetails, type RedirectProblemDetails } from '../../../models/index.js';
// @ts-ignore
import { BookmarksRequestBuilderNavigationMetadata, type BookmarksRequestBuilder } from './bookmarks/index.js';
// @ts-ignore
import { ProgressRequestBuilderNavigationMetadata, type ProgressRequestBuilder } from './progress/index.js';
// @ts-ignore
import { type UpdateRequestBuilder, UpdateRequestBuilderRequestsMetadata } from './update/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /web-api/books/{bookId}
 */
export interface WithBookItemRequestBuilder extends BaseRequestBuilder<WithBookItemRequestBuilder> {
    /**
     * The bookmarks property
     */
    get bookmarks(): BookmarksRequestBuilder;
    /**
     * The progress property
     */
    get progress(): ProgressRequestBuilder;
    /**
     * The update property
     */
    get update(): UpdateRequestBuilder;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<ArrayBuffer>}
     * @throws {ProblemDetails} error when the service returns a 400 status code
     * @throws {RedirectProblemDetails} error when the service returns a 401 status code
     */
     delete(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<ArrayBuffer | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<BookInfo>}
     * @throws {ProblemDetails} error when the service returns a 400 status code
     * @throws {RedirectProblemDetails} error when the service returns a 401 status code
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<BookInfo | undefined>;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toDeleteRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
    /**
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const WithBookItemRequestBuilderUriTemplate = "{+baseurl}/web-api/books/{bookId}";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const WithBookItemRequestBuilderNavigationMetadata: Record<Exclude<keyof WithBookItemRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    bookmarks: {
        navigationMetadata: BookmarksRequestBuilderNavigationMetadata,
    },
    progress: {
        navigationMetadata: ProgressRequestBuilderNavigationMetadata,
    },
    update: {
        requestsMetadata: UpdateRequestBuilderRequestsMetadata,
    },
};
/**
 * Metadata for all the requests in the request builder.
 */
export const WithBookItemRequestBuilderRequestsMetadata: RequestsMetadata = {
    delete: {
        uriTemplate: WithBookItemRequestBuilderUriTemplate,
        responseBodyContentType: "application/problem+json",
        errorMappings: {
            400: createProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createRedirectProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "ArrayBuffer",
    },
    get: {
        uriTemplate: WithBookItemRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        errorMappings: {
            400: createProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createRedirectProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "send",
        responseBodyFactory:  createBookInfoFromDiscriminatorValue,
    },
};
/* tslint:enable */
/* eslint-enable */
