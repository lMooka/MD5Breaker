using MD5Breaker.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MD5Breaker.Core
{
    public class MD5Decrypter
    {
        public static string currentPassword;
        public static string currentHashPassword;
        private string initialHash = "";

        public static char[] CharRange = {
                                            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'k', 'x', 'w', 'y', 'z',
                                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'K', 'X', 'W', 'Y','Z',
                                            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                            '@', '#', '$', '%', '*', '+', '-'
                                        };

        public string Hash { get; private set; }
        private DecrypterRange Range;
        private MD5 md5;

        public MD5Decrypter(string hash, DecrypterRange range)
        {
            this.Hash = hash;
            this.Range = range;
            md5 = MD5.Create();
        }

        private string GenerateSeqPassword()
        {
            Range.Next();

            string s = "";
            foreach (int r in Range.currentRange)
            {
                s += CharRange[r];
            }

            return s;
        }

        public void Crack()
        {
            string gpw = GenerateSeqPassword();
            string hash = GetMd5Hash(md5, gpw);

            currentPassword = gpw;
            currentHashPassword = hash;

            if (Hash == hash)
                throw new HashFoundException(md5 + " : " + gpw);

            if (initialHash == hash)
                throw new HashNotFoundException();

            if (initialHash == "")
                initialHash = hash;
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
