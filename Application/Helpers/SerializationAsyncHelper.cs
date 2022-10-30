using System.IO;
using System.Threading.Tasks;
using Depra.Serialization.Domain.Serializers;

namespace Depra.Serialization.Application.Helpers
{
    internal static class SerializationAsyncHelper
    {
        public static Task SerializeAsync<TIn>(ISerializer serializer, Stream outputStream, TIn input) =>
            Task.Run(() => serializer.Serialize(outputStream, input));

        public static Task<TOut> DeserializeAsync<TOut>(ISerializer serializer, Stream inputStream)
        {
            var deserializedObject = serializer.Deserialize<TOut>(inputStream);
            return Task.FromResult(deserializedObject);
        }
    }
}