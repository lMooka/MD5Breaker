using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Networking.Serialization
{
    public static class GenericSerializer
    {
        public static byte[] GetBinary(object obj)
        {
            MemoryStream ms = new MemoryStream();

            new BinaryFormatter().Serialize(ms, obj);
            return ms.ToArray();
        }

        public static T GetObject<T>(byte[] bytes)
        {
            using (MemoryStream memStream = new MemoryStream(bytes))
            {
                return (T)new BinaryFormatter().Deserialize(memStream);
            }
        }

        public static int GetByteLength(object obj)
        {
            MemoryStream ms = new MemoryStream();

            new BinaryFormatter().Serialize(ms, obj);
            return ms.ToArray().Length;
        }
    }
}
