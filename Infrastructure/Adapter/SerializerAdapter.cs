// Copyright © 2022 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define CSHARP8_OR_GREATER
#endif
using System;
using System.IO;
using System.Threading.Tasks;
using Depra.Serialization.Application.Errors;
using Depra.Serialization.Application.Interfaces;

namespace Depra.Serialization.Infrastructure.Adapter
{
    /// <summary>
    /// Adapter class for third party serializers.
    /// </summary>
    public abstract class SerializerAdapter : ISerializationProvider
    {
        /// <inheritdoc />
        public abstract byte[] Serialize<TIn>(TIn input);

        /// <inheritdoc />
        public abstract void Serialize<TIn>(Stream outputStream, TIn input);

        /// <inheritdoc />
        public abstract Task SerializeAsync<TIn>(Stream outputStream, TIn input);

        /// <inheritdoc />
        public abstract string SerializeToPrettyString<TIn>(TIn input);

        /// <inheritdoc />
        public abstract string SerializeToString<TIn>(TIn input);

        /// <inheritdoc />
        public abstract TOut Deserialize<TOut>(string input);

        /// <inheritdoc />
        public abstract TOut Deserialize<TOut>(Stream inputStream);

#if CSHARP8_OR_GREATER
        /// <inheritdoc />
        public abstract TOut Deserialize<TOut>(ReadOnlyMemory<byte> input);
#endif

        /// <inheritdoc />
        public abstract Task<TOut> DeserializeAsync<TOut>(Stream inputStream);

        protected static void ThrowIfNullOrEmpty(string input, string argumentName) =>
            Guard.AgainstNullOrEmpty(input, argumentName);

        protected static void ThrowIfNullOrEmpty(Stream inputStream, string argumentName) =>
            Guard.AgainstNullOrEmpty(inputStream, argumentName);

        protected static void ThrowIfEmpty(ArraySegment<byte> input, string argumentName) =>
            Guard.AgainstEmpty(input, argumentName);
    }
}