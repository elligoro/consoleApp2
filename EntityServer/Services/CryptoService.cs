using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Collections;

namespace EntityServer.Services
{
    public static class CryptoService
    {

        public static string GetRandomCryptoString()
        {
            var buffer = new byte[20];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {               
                rngCsp.GetBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        public static string HashSHA256(string msg)
        {
            using(var hash256 = SHA256.Create())
            return GetHash(hash256,msg);
        } 
        private static string GetHash(HashAlgorithm hashAlgorithm, string msg)
        {
            var dataBytes = Encoding.UTF8.GetBytes(msg);
            byte[] dataHash = hashAlgorithm.ComputeHash(dataBytes);

            return BitConverter.ToString(dataHash).Replace("-","");
        }
        public static bool VerifyHash(string input, string stored)
        {
            stored = stored.ToLower();
            input = input.ToLower();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var storedBytes = Encoding.UTF8.GetBytes(stored);
            return CompareBytes(inputBytes, storedBytes);
        }
        private static bool CompareBytes(byte[] buffer1, byte[] buffer2)
        {
            var bitArray1 = new BitArray(buffer1);
            var bitArray2 = new BitArray(buffer2);
            var res = false;
            for(var i=0; bitArray1.Length > i; i++)
            {
                res |= (bitArray1[i] ^ bitArray2[i]);
            }
            return res == false;
        }

        public static string ApplyHmac(string token, string salt) => Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                                                                                                  password: token,
                                                                                                                  salt: Convert.FromBase64String(salt),
                                                                                                                  prf: KeyDerivationPrf.HMACSHA256,
                                                                                                                  iterationCount: 100000,
                                                                                                                  numBytesRequested: 256/8
                                                                                                                ));
    }
}
