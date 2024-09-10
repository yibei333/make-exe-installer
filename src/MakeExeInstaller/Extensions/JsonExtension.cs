using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MakeExeInstaller.Extensions
{
    public static class JsonExtension
    {
        public static string Serialize(this object instance)
        {
            var serializer = new DataContractJsonSerializer(instance.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, instance);
                return Encoding.Default.GetString(stream.ToArray());
            }
        }

        public static T DeSerialize<T>(this string json) where T : class
        {
            using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                return serializer.ReadObject(stream) as T;
            }
        }
    }
}
