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
            using (MemoryStream memStream = new MemoryStream(bytes.Length))
            {

                memStream.Write(bytes, 0, bytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);

                return (T)new BinaryFormatter().Deserialize(memStream);
            }
        }
    }
}
