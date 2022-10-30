// Copyright © 2022 Nikolay Melnikov. All rights reserved.
// SPDX-License-Identifier: Apache-2.0

using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Depra.Serialization.Infrastructure.Adapter;

namespace Depra.Serialization.Json.Microsoft
{
    public sealed class MicrosoftJsonSerializer : SerializerAdapter
    {
        private readonly JsonSerializerOptions _options;

        public override byte[] Serialize<TIn>(TIn input) => JsonSerializer.SerializeToUtf8Bytes(input, _options);

        public override void Serialize<TIn>(Stream outputStream, TIn input)
        {
            JsonSerializer.Serialize(new Utf8JsonWriter(outputStream), input, _options);
            outputStream.Seek(0, SeekOrigin.Begin);
        }

        public override async Task SerializeAsync<TIn>(Stream outputStream, TIn input) =>
            await JsonSerializer.SerializeAsync(outputStream, input, _options).ConfigureAwait(false);

        public override string SerializeToPrettyString<TIn>(TIn input) =>
            JsonSerializer.Serialize(input, new JsonSerializerOptions {WriteIndented = true});

        public override string SerializeToString<TIn>(TIn input) => JsonSerializer.Serialize(input, _options);

        public override TOut Deserialize<TOut>(string input)
        {
            ThrowIfNullOrEmpty(input, nameof(input));
            
            return JsonSerializer.Deserialize<TOut>(Encoding.UTF8.GetBytes(input), _options);
        }

        public override TOut Deserialize<TOut>(ReadOnlyMemory<byte> input)
        {
            ThrowIfEmpty(input.ToArray(), nameof(input));

            return JsonSerializer.Deserialize<TOut>(input.Span, _options);
        }

        public override TOut Deserialize<TOut>(Stream inputStream)
        {
            ThrowIfNullOrEmpty(inputStream, nameof(inputStream));

            if (inputStream.Position == inputStream.Length)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            var length = (int) inputStream.Length;
            var buffer = new Span<byte>(new byte[length]);
            var bytesRead = inputStream.Read(buffer);
            if (bytesRead == 0)
            {
                throw new InvalidDataException();
            }

            var utf8Reader = new Utf8JsonReader(buffer);
            var deserializedObject = JsonSerializer.Deserialize<TOut>(ref utf8Reader, _options);

            return deserializedObject;
        }

        public override async Task<TOut> DeserializeAsync<TOut>(Stream inputStream)
        {
            ThrowIfNullOrEmpty(inputStream, nameof(inputStream));

            if (inputStream.Position == inputStream.Length)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            return await JsonSerializer.DeserializeAsync<TOut>(inputStream, _options).ConfigureAwait(false);
        }

        public MicrosoftJsonSerializer(JsonSerializerOptions options = null) => _options = options;

        public override string ToString() => typeof(JsonSerializer).Namespace;
    }
}