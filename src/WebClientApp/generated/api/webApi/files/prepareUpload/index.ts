/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createPreparedFileUploadResultFromDiscriminatorValue, createProblemDetailsFromDiscriminatorValue, createRedirectProblemDetailsFromDiscriminatorValue, serializePreparedFileUploadResult, serializePrepareFileUploadRequest, type PreparedFileUploadResult, type PrepareFileUploadRequest, type ProblemDetails, type RedirectProblemDetails } from '../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /web-api/files/prepare-upload
 */
export interface PrepareUploadRequestBuilder extends BaseRequestBuilder<PrepareUploadRequestBuilder> {
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<PreparedFileUploadResult>}
     * @throws {ProblemDetails} error when the service returns a 400 status code
     * @throws {RedirectProblemDetails} error when the service returns a 401 status code
     */
     put(body: PrepareFileUploadRequest, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<PreparedFileUploadResult | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toPutRequestInformation(body: PrepareFileUploadRequest, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const PrepareUploadRequestBuilderUriTemplate = "{+baseurl}/web-api/files/prepare-upload";
/**
 * Metadata for all the requests in the request builder.
 */
export const PrepareUploadRequestBuilderRequestsMetadata: RequestsMetadata = {
    put: {
        uriTemplate: PrepareUploadRequestBuilderUriTemplate,
        responseBodyContentType: "application/json, text/plain;q=0.9",
        errorMappings: {
            400: createProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createRedirectProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "send",
        responseBodyFactory:  createPreparedFileUploadResultFromDiscriminatorValue,
        requestBodyContentType: "application/json",
        requestBodySerializer: serializePrepareFileUploadRequest,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
