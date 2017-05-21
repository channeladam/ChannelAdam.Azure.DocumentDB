//-----------------------------------------------------------------------
// <copyright file="OptimisticLockException.cs">
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

using Microsoft.Azure.Documents;
using System;
using System.Runtime.Serialization;

namespace ChannelAdam.Azure.DocumentDB
{
    [Serializable]
    public class DocumentOptimisticLockException : Exception
    {
        /// <summary>
        /// The actual <see cref="Document"/> - only when the Document is known. Either Document or DocumentObject will be set.
        /// </summary>
        public Document Document { get; set; }

        /// <summary>
        /// The plain object representing the content of the document - only when it is known. Not a <see cref="Document"/>. Either Document or DocumentObject will be set.
        /// </summary>
        public object DocumentObject { get; set; }

        /// <summary>
        /// The self link of the document. 
        /// </summary>
        public string DocumentLink { get; set; }

        /// <summary>
        /// The eTag that was expected.
        /// </summary>
        public string ETag { get; set; }

        public DocumentOptimisticLockException()
        {
        }

        public DocumentOptimisticLockException(string message) : base(message)
        {
        }

        public DocumentOptimisticLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DocumentOptimisticLockException(string message, Document document, Exception innerException) : base(message, innerException)
        {
            this.Document = document;
            this.ETag = document.ETag;
            this.DocumentLink = document.SelfLink;
        }

        public DocumentOptimisticLockException(string message, string documentLink, string eTag, object documentObject, Exception innerException) : base(message, innerException)
        {
            this.DocumentObject = documentObject;
            this.DocumentLink = documentLink;
            this.ETag = eTag;
        }

        protected DocumentOptimisticLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}