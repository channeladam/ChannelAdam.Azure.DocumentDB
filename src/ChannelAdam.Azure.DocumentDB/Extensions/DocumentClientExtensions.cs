//-----------------------------------------------------------------------
// <copyright file="DocumentClientExtensions.cs">
//     Copyright (c) 2017 Adam Craven. All rights reserved.
// </copyright>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Microsoft.Azure.Documents.Client
{
    /// <summary>
    /// Provides extension methods for the Azure DocumentDB Client - <see cref="DocumentClient"/>.
    /// </summary>
    public static class DocumentClientExtensions
    {
        #region Public Methods

        /// <summary>
        /// Retrieves the specified document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="documentId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the document does not exist.</returns>
        public static Task<ResourceResponse<Document>> TryReadDocumentAsync(this DocumentClient client, string databaseId, string collectionId, string documentId, RequestOptions requestOptions = null, ILogger logger = null)
        {
            var documentUri = UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);
            return TryReadDocumentAsync(client, documentUri, requestOptions, logger);
        }

        /// <summary>
        /// Retrieves the specified document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="documentUri"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the document does not exist.</returns>
        public static async Task<ResourceResponse<Document>> TryReadDocumentAsync(this DocumentClient client, Uri documentUri, RequestOptions requestOptions = null, ILogger logger = null)
        {
            ResourceResponse<Document> result = null;

            try
            {
                result = await client.ReadDocumentAsync(documentUri, requestOptions).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogTrace($"Document '{documentUri.OriginalString}' does not exist");
            }

            return result;
        }

        /// <summary>
        /// Retrieve the specified document.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="documentId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the document does not exist.</returns>
        public static Task<DocumentResponse<TDocument>> TryReadDocumentAsync<TDocument>(this DocumentClient client, string databaseId, string collectionId, string documentId, RequestOptions requestOptions = null, ILogger logger = null) where TDocument : class, new()
        {
            var documentUri = UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);
            return TryReadDocumentAsync<TDocument>(client, documentUri, requestOptions, logger);
        }

        /// <summary>
        /// Retrieve the specified document.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="client"></param>
        /// <param name="documentUri"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the document does not exist.</returns>
        public static async Task<DocumentResponse<TDocument>> TryReadDocumentAsync<TDocument>(this DocumentClient client, Uri documentUri, RequestOptions requestOptions = null, ILogger logger = null) where TDocument : class, new()
        {
            DocumentResponse<TDocument> result = null;

            try
            {
                result = await client.ReadDocumentAsync<TDocument>(documentUri, requestOptions).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogTrace($"Document '{documentUri.OriginalString}' does not exist");
            }

            return result;
        }

        /// <summary>
        /// Try to delete the document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="disableAutomaticIdGeneration"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the create failed due to a conflict (i.e. the document already exists).</returns>
        public static Task<ResourceResponse<Document>> TryCreateDocumentAsync(this DocumentClient client, string databaseId, string collectionId, object document, RequestOptions requestOptions = null, bool disableAutomaticIdGeneration = false, ILogger logger = null)
        {
            return TryCreateDocumentAsync(client, UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document, requestOptions, disableAutomaticIdGeneration, logger);
        }

        /// <summary>
        /// Try to create the document.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="client"></param>
        /// <param name="collectionUri"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="disableAutomaticIdGeneration"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the create failed due to a conflict (i.e. the document already exists).</returns>
        public static async Task<ResourceResponse<Document>> TryCreateDocumentAsync(this DocumentClient client, Uri collectionUri, object document, RequestOptions requestOptions = null, bool disableAutomaticIdGeneration = false, ILogger logger = null)
        {
            ResourceResponse<Document> result = null;

            try
            {
                result = await client.CreateDocumentAsync(collectionUri, document, requestOptions, disableAutomaticIdGeneration).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.Conflict)
            {
                logger?.LogTrace($"Document already exists in collection '{collectionUri.OriginalString}'");
            }

            return result;
        }

        /// <summary>
        /// Try to delete the document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="documentId"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static Task<ResourceResponse<Document>> TryDeleteDocumentAsync(this DocumentClient client, string databaseId, string collectionId, string documentId, RequestOptions requestOptions = null, ILogger logger = null)
        {
            return TryDeleteDocumentAsync(client, UriFactory.CreateDocumentUri(databaseId, collectionId, documentId), requestOptions, logger);
        }

        /// <summary>
        /// Try to delete the document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="documentUri"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the delete failed because the document did not already exist.</returns>
        public static async Task<ResourceResponse<Document>> TryDeleteDocumentAsync(this DocumentClient client, Uri documentUri, RequestOptions requestOptions = null, ILogger logger = null)
        {
            ResourceResponse<Document> result = null;

            try
            {
                result = await client.DeleteDocumentAsync(documentUri, requestOptions).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogTrace($"Document '{documentUri.OriginalString}' does not exist");
            }

            return result;
        }

        /// <summary>
        /// Try to replace the document.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="client"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the delete failed because the document did not already exist.</returns>
        public static async Task<ResourceResponse<Document>> TryReplaceDocumentAsync<TDocument>(this DocumentClient client, TDocument document, RequestOptions requestOptions = null, ILogger logger = null) where TDocument : Document
        {
            ResourceResponse<Document> result = null;

            try
            {
                result = await client.ReplaceDocumentAsync(document, requestOptions).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogTrace($"Document '{document.Id}' does not exist - so it cannot be replaced");
            }

            return result;
        }

        /// <summary>
        /// Upsert the given document.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="disableAutomaticIdGeneration"></param>
        /// <returns></returns>
        public static Task<ResourceResponse<Document>> UpsertDocumentAsync(this DocumentClient client, string databaseId, string collectionId, object document, RequestOptions requestOptions = null, bool disableAutomaticIdGeneration = false)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            return client.UpsertDocumentAsync(collectionUri, document, requestOptions, disableAutomaticIdGeneration);
        }

        /// <summary>
        /// Upsert the given collection of documents individually.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="documents"></param>
        /// <param name="requestOptions"></param>
        /// <param name="disableAutomaticIdGeneration"></param>
        /// <returns></returns>
        /// <remarks>
        /// Beware of the cost and speed of using this client approach (which has the client calling the database once per document)
        ///     - as opposed to making one call to a stored procedure to upsert a batch of documents via one server operation.
        /// </remarks>
        public static async Task<ICollection<ResourceResponse<Document>>> UpsertDocumentsIndividuallyAsync<TDocument>(this DocumentClient client, string databaseId, string collectionId, ICollection<TDocument> documents, RequestOptions requestOptions = null, bool disableAutomaticIdGeneration = false) where TDocument : Document
        {
            var result = new List<ResourceResponse<Document>>();

            foreach (var document in documents)
            {
                var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
                result.Add(await client.UpsertDocumentAsync(collectionUri, document, requestOptions, disableAutomaticIdGeneration).ConfigureAwait(false));
            }

            return result;
        }

        #endregion Public Methods
    }
}
