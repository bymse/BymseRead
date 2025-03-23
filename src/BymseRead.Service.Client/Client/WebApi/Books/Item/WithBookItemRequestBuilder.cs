// <auto-generated/>
#pragma warning disable CS0618
using BymseRead.Service.Client.Models;
using BymseRead.Service.Client.WebApi.Books.Item.Bookmarks;
using BymseRead.Service.Client.WebApi.Books.Item.Progress;
using BymseRead.Service.Client.WebApi.Books.Item.Update;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace BymseRead.Service.Client.WebApi.Books.Item
{
    /// <summary>
    /// Builds and executes requests for operations under \web-api\books\{bookId}
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class WithBookItemRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The bookmarks property</summary>
        public global::BymseRead.Service.Client.WebApi.Books.Item.Bookmarks.BookmarksRequestBuilder Bookmarks
        {
            get => new global::BymseRead.Service.Client.WebApi.Books.Item.Bookmarks.BookmarksRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The progress property</summary>
        public global::BymseRead.Service.Client.WebApi.Books.Item.Progress.ProgressRequestBuilder Progress
        {
            get => new global::BymseRead.Service.Client.WebApi.Books.Item.Progress.ProgressRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The update property</summary>
        public global::BymseRead.Service.Client.WebApi.Books.Item.Update.UpdateRequestBuilder Update
        {
            get => new global::BymseRead.Service.Client.WebApi.Books.Item.Update.UpdateRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::BymseRead.Service.Client.WebApi.Books.Item.WithBookItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithBookItemRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/web-api/books/{bookId}", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::BymseRead.Service.Client.WebApi.Books.Item.WithBookItemRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public WithBookItemRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/web-api/books/{bookId}", rawUrl)
        {
        }
        /// <returns>A <see cref="Stream"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::BymseRead.Service.Client.Models.ProblemDetails">When receiving a 400 status code</exception>
        /// <exception cref="global::BymseRead.Service.Client.Models.RedirectProblemDetails">When receiving a 401 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<Stream?> DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<Stream> DeleteAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToDeleteRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::BymseRead.Service.Client.Models.ProblemDetails.CreateFromDiscriminatorValue },
                { "401", global::BymseRead.Service.Client.Models.RedirectProblemDetails.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendPrimitiveAsync<Stream>(requestInfo, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <returns>A <see cref="global::BymseRead.Service.Client.Models.BookInfo"/></returns>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
        /// <exception cref="global::BymseRead.Service.Client.Models.ProblemDetails">When receiving a 400 status code</exception>
        /// <exception cref="global::BymseRead.Service.Client.Models.RedirectProblemDetails">When receiving a 401 status code</exception>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::BymseRead.Service.Client.Models.BookInfo?> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::BymseRead.Service.Client.Models.BookInfo> GetAsync(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            var requestInfo = ToGetRequestInformation(requestConfiguration);
            var errorMapping = new Dictionary<string, ParsableFactory<IParsable>>
            {
                { "400", global::BymseRead.Service.Client.Models.ProblemDetails.CreateFromDiscriminatorValue },
                { "401", global::BymseRead.Service.Client.Models.RedirectProblemDetails.CreateFromDiscriminatorValue },
            };
            return await RequestAdapter.SendAsync<global::BymseRead.Service.Client.Models.BookInfo>(requestInfo, global::BymseRead.Service.Client.Models.BookInfo.CreateFromDiscriminatorValue, errorMapping, cancellationToken).ConfigureAwait(false);
        }
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToDeleteRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.DELETE, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/problem+json");
            return requestInfo;
        }
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToGetRequestInformation(Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            var requestInfo = new RequestInformation(Method.GET, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json, text/plain;q=0.9");
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::BymseRead.Service.Client.WebApi.Books.Item.WithBookItemRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::BymseRead.Service.Client.WebApi.Books.Item.WithBookItemRequestBuilder WithUrl(string rawUrl)
        {
            return new global::BymseRead.Service.Client.WebApi.Books.Item.WithBookItemRequestBuilder(rawUrl, RequestAdapter);
        }
        /// <summary>
        /// Configuration for the request such as headers, query parameters, and middleware options.
        /// </summary>
        [Obsolete("This class is deprecated. Please use the generic RequestConfiguration class generated by the generator.")]
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class WithBookItemRequestBuilderDeleteRequestConfiguration : RequestConfiguration<DefaultQueryParameters>
        {
        }
        /// <summary>
        /// Configuration for the request such as headers, query parameters, and middleware options.
        /// </summary>
        [Obsolete("This class is deprecated. Please use the generic RequestConfiguration class generated by the generator.")]
        [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
        public partial class WithBookItemRequestBuilderGetRequestConfiguration : RequestConfiguration<DefaultQueryParameters>
        {
        }
    }
}
#pragma warning restore CS0618
