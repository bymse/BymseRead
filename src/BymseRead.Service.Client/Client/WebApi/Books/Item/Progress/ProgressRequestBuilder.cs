// <auto-generated/>
#pragma warning disable CS0618
using BymseRead.Service.Client.WebApi.Books.Item.Progress.CurrentPage;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace BymseRead.Service.Client.WebApi.Books.Item.Progress
{
    /// <summary>
    /// Builds and executes requests for operations under \web-api\books\{bookId}\progress
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ProgressRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The currentPage property</summary>
        public global::BymseRead.Service.Client.WebApi.Books.Item.Progress.CurrentPage.CurrentPageRequestBuilder CurrentPage
        {
            get => new global::BymseRead.Service.Client.WebApi.Books.Item.Progress.CurrentPage.CurrentPageRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::BymseRead.Service.Client.WebApi.Books.Item.Progress.ProgressRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ProgressRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/web-api/books/{bookId}/progress", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::BymseRead.Service.Client.WebApi.Books.Item.Progress.ProgressRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ProgressRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/web-api/books/{bookId}/progress", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
