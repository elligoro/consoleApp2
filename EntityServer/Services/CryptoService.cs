using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace EntityServer.Services
{
    public static class CryptoService
    {
        public static string HashSHA256(string msg)
        {
            using(var hash256 = SHA256.Create())
            return GetHash(hash256,msg);
        } 
        public static string GetHash(HashAlgorithm hashAlgorithm, string msg)
        {
            var dataBytes = Encoding.UTF8.GetBytes(msg);
            byte[] dataHash = hashAlgorithm.ComputeHash(dataBytes);

            var hexArr = dataHash.Select(b => b.ToString("x2"));
            return string.Join("", hexArr);
        }
        public static bool VerifyHash(HashAlgorithm hashAlgorithm, string hash, string msg)
        {
            var hashedMsg = GetHash(hashAlgorithm, msg);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashedMsg, msg) == 0;
        }
    }
}
