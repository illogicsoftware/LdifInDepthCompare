using System;
using System.Security.Cryptography;
using System.Text;

namespace LdifInDepthCompare
{
    public class Crypto
    {
        private byte[] calculateHash(string inputString)
        {
            HashAlgorithm algorithm = MD5.Create();  //or use SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string getHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in calculateHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
