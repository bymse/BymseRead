/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createProblemDetailsFromDiscriminatorValue, createRedirectProblemDetailsFromDiscriminatorValue, serializeUpdateBookRequest, type ProblemDetails, type RedirectProblemDetails, type UpdateBookRequest } from '../../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /web-api/books/{bookId}/update
 */
export interface UpdateRequestBuilder extends BaseRequestBuilder<UpdateRequestBuilder> {
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<ArrayBuffer>}
     * @throws {ProblemDetails} error when the service returns a 400 status code
     * @throws {RedirectProblemDetails} error when the service returns a 401 status code
     */
     post(body: UpdateBookRequest, requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<ArrayBuffer | undefined>;
    /**
     * @param body The request body
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toPostRequestInformation(body: UpdateBookRequest, requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const UpdateRequestBuilderUriTemplate = "{+baseurl}/web-api/books/{bookId}/update";
/**
 * Metadata for all the requests in the request builder.
 */
export const UpdateRequestBuilderRequestsMetadata: RequestsMetadata = {
    post: {
        uriTemplate: UpdateRequestBuilderUriTemplate,
        responseBodyContentType: "application/problem+json",
        errorMappings: {
            400: createProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createRedirectProblemDetailsFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "sendPrimitive",
        responseBodyFactory:  "ArrayBuffer",
        requestBodyContentType: "application/json",
        requestBodySerializer: serializeUpdateBookRequest,
        requestInformationContentSetMethod: "setContentFromParsable",
    },
};
/* tslint:enable */
/* eslint-enable */
