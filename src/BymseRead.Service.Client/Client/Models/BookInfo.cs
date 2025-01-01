// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace BymseRead.Service.Client.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class BookInfo : IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The bookFile property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::BymseRead.Service.Client.Models.FileInfo? BookFile { get; set; }
#nullable restore
#else
        public global::BymseRead.Service.Client.Models.FileInfo BookFile { get; set; }
#endif
        /// <summary>The bookId property</summary>
        public Guid? BookId { get; set; }
        /// <summary>The coverUrl property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? CoverUrl { get; set; }
#nullable restore
#else
        public string CoverUrl { get; set; }
#endif
        /// <summary>The currentPage property</summary>
        public int? CurrentPage { get; set; }
        /// <summary>The lastBookmark property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::BymseRead.Service.Client.Models.BookmarkInfo? LastBookmark { get; set; }
#nullable restore
#else
        public global::BymseRead.Service.Client.Models.BookmarkInfo LastBookmark { get; set; }
#endif
        /// <summary>The pages property</summary>
        public int? Pages { get; set; }
        /// <summary>The title property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::BymseRead.Service.Client.Models.BookInfo"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::BymseRead.Service.Client.Models.BookInfo CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::BymseRead.Service.Client.Models.BookInfo();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "bookFile", n => { BookFile = n.GetObjectValue<global::BymseRead.Service.Client.Models.FileInfo>(global::BymseRead.Service.Client.Models.FileInfo.CreateFromDiscriminatorValue); } },
                { "bookId", n => { BookId = n.GetGuidValue(); } },
                { "coverUrl", n => { CoverUrl = n.GetStringValue(); } },
                { "currentPage", n => { CurrentPage = n.GetIntValue(); } },
                { "lastBookmark", n => { LastBookmark = n.GetObjectValue<global::BymseRead.Service.Client.Models.BookmarkInfo>(global::BymseRead.Service.Client.Models.BookmarkInfo.CreateFromDiscriminatorValue); } },
                { "pages", n => { Pages = n.GetIntValue(); } },
                { "title", n => { Title = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::BymseRead.Service.Client.Models.FileInfo>("bookFile", BookFile);
            writer.WriteGuidValue("bookId", BookId);
            writer.WriteStringValue("coverUrl", CoverUrl);
            writer.WriteIntValue("currentPage", CurrentPage);
            writer.WriteObjectValue<global::BymseRead.Service.Client.Models.BookmarkInfo>("lastBookmark", LastBookmark);
            writer.WriteIntValue("pages", Pages);
            writer.WriteStringValue("title", Title);
        }
    }
}
#pragma warning restore CS0618
