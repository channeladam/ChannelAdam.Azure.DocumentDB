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

using System;
using System.Runtime.Serialization;

namespace ChannelAdam.Azure.DocumentDB
{
    [Serializable]
    public class OptimisticLockException : Exception
    {
        public OptimisticLockException()
        {
        }

        public OptimisticLockException(string message) : base(message)
        {
        }

        public OptimisticLockException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OptimisticLockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}