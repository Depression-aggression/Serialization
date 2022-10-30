﻿// Copyright © 2022 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Depra.Serialization.Application.Helpers;
using Depra.Serialization.Domain.Serializers;

namespace Depra.Serialization.Application.Binary
{
    [Obsolete]
    public sealed class BinarySerializer : ISerializer
    {
        private static readonly Encoding ENCODING_TYPE = Encoding.ASCII;

        private readonly BinaryFormatter _binaryFormatter;

        public BinarySerializer() => _binaryFormatter = new BinaryFormatter();

        public byte[] Serialize<TIn>(TIn input) => SerializationHelper.SerializeToBytes(this, input);

        public void Serialize<TIn>(Stream outputStream, TIn input) => _binaryFormatter.Serialize(outputStream, input);

        public Task SerializeAsync<TIn>(Stream outputStream, TIn input) =>
            SerializationAsyncHelper.SerializeAsync(this, outputStream, input);

        public string SerializeToPrettyString<TIn>(TIn input) => SerializeToString(input);

        public string SerializeToString<TIn>(TIn input) =>
            SerializationHelper.SerializeToString(this, input, ENCODING_TYPE);

        public TOut Deserialize<TOut>(string input) =>
            SerializationHelper.DeserializeFromString<TOut>(this, input, ENCODING_TYPE);

        public TOut Deserialize<TOut>(Stream inputStream)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
            return (TOut) _binaryFormatter.Deserialize(inputStream);
        }

        public Task<TOut> DeserializeAsync<TOut>(Stream inputStream) =>
            SerializationAsyncHelper.DeserializeAsync<TOut>(this, inputStream);

        public override string ToString() => nameof(BinarySerializer);
    }
}