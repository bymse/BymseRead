/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { PrepareUploadRequestBuilderRequestsMetadata, type PrepareUploadRequestBuilder } from './prepareUpload/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type KeysToExcludeForNavigationMetadata, type NavigationMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /web-api/files
 */
export interface FilesRequestBuilder extends BaseRequestBuilder<FilesRequestBuilder> {
    /**
     * The prepareUpload property
     */
    get prepareUpload(): PrepareUploadRequestBuilder;
}
/**
 * Uri template for the request builder.
 */
export const FilesRequestBuilderUriTemplate = "{+baseurl}/web-api/files";
/**
 * Metadata for all the navigation properties in the request builder.
 */
export const FilesRequestBuilderNavigationMetadata: Record<Exclude<keyof FilesRequestBuilder, KeysToExcludeForNavigationMetadata>, NavigationMetadata> = {
    prepareUpload: {
        requestsMetadata: PrepareUploadRequestBuilderRequestsMetadata,
    },
};
/* tslint:enable */
/* eslint-enable */
