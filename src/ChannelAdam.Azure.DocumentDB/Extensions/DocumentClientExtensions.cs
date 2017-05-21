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

using ChannelAdam.Azure.DocumentDB;
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
        /// Try to replace the document, using optimistic locking on the document's eTag.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the replace failed because the document did not already exist.</returns>
        /// <exception cref="OptimisticLockException"></exception>
        public static Task<ResourceResponse<Document>> TryReplaceDocumentWithOptimisticLockingAsync(this DocumentClient client, Document document, RequestOptions requestOptions = null, ILogger logger = null)
        {
            requestOptions = requestOptions ?? new RequestOptions();
            requestOptions.AccessCondition = new AccessCondition { Condition = document.ETag, Type = AccessConditionType.IfMatch };

            try
            {
                return TryReplaceDocumentAsync(client, document, requestOptions, logger);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                string error = $"Optimistic locking error occurred while replacing document '{document.SelfLink}'. Expected eTag {document.ETag}.";
                logger?.LogTrace(error);
                throw new OptimisticLockException(error, dcex);
            }
        }

        /// <summary>
        /// Try to replace the document, using optimistic locking with the given eTag.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="documentId"></param>
        /// <param name="eTag"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the replace failed because the document did not already exist.</returns>
        /// <exception cref="OptimisticLockException"></exception>
        public static Task<ResourceResponse<Document>> TryReplaceDocumentWithOptimisticLockingAsync(this DocumentClient client, string databaseId, string collectionId, string documentId, string eTag, object document, RequestOptions requestOptions = null, ILogger logger = null)
        {
            requestOptions = requestOptions ?? new RequestOptions();
            requestOptions.AccessCondition = new AccessCondition { Condition = eTag, Type = AccessConditionType.IfMatch };

            var documentUri = UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);

            try
            {
                return TryReplaceDocumentAsync(client, documentUri, document, requestOptions, logger);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.PreconditionFailed)
            {
                string error = $"Optimistic locking error occurred while replacing document '{documentUri.OriginalString}'. Expected eTag {eTag}.";
                logger?.LogTrace(error);
                throw new OptimisticLockException(error, dcex);
            }
        }

        /// <summary>
        /// Try to replace the document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the replace failed because the document did not already exist.</returns>
        public static async Task<ResourceResponse<Document>> TryReplaceDocumentAsync(this DocumentClient client, Document document, RequestOptions requestOptions = null, ILogger logger = null)
        {
            ResourceResponse<Document> result = null;

            try
            {
                result = await client.ReplaceDocumentAsync(document, requestOptions).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogTrace($"Document '{document.SelfLink}' does not exist - so it cannot be replaced");
            }

            return result;
        }

        /// <summary>
        /// Try to replace the document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="databaseId"></param>
        /// <param name="collectionId"></param>
        /// <param name="documentId"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the replace failed because the document did not already exist.</returns>
        public static Task<ResourceResponse<Document>> TryReplaceDocumentAsync(this DocumentClient client, string databaseId, string collectionId, string documentId, object document, RequestOptions requestOptions = null, ILogger logger = null)
        {
            return TryReplaceDocumentAsync(client, UriFactory.CreateDocumentUri(databaseId, collectionId, documentId), document, requestOptions, logger);
        }
        
        /// <summary>
        /// Try to replace the document.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="documentUri"></param>
        /// <param name="document"></param>
        /// <param name="requestOptions"></param>
        /// <param name="logger"></param>
        /// <returns>Null if the replace failed because the document did not already exist.</returns>
        public static async Task<ResourceResponse<Document>> TryReplaceDocumentAsync(this DocumentClient client, Uri documentUri, object document, RequestOptions requestOptions = null, ILogger logger = null)
        {
            ResourceResponse<Document> result = null;

            try
            {
                result = await client.ReplaceDocumentAsync(documentUri, document, requestOptions).ConfigureAwait(false);
            }
            catch (DocumentClientException dcex) when (dcex.StatusCode == HttpStatusCode.NotFound)
            {
                logger?.LogTrace($"Document '{documentUri.OriginalString}' does not exist - so it cannot be replaced");
            }

            return result;
        }
         
        /// <summary>
        /// Upsert the given document.
        /// </summary>
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
        public static async Task<ICollection<ResourceResponse<Document>>> UpsertDocumentsIndividuallyAsync(this DocumentClient client, string databaseId, string collectionId, ICollection<object> documents, RequestOptions requestOptions = null, bool disableAutomaticIdGeneration = false)
        {
            var result = new List<ResourceResponse<Document>>();
            var collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

            foreach (var document in documents)
            {
                result.Add(await client.UpsertDocumentAsync(collectionUri, document, requestOptions, disableAutomaticIdGeneration).ConfigureAwait(false));
            }

            return result;
        }

        #endregion Public Methods
    }
}
