using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Handlers
{
    public class HashHandler
    {
        public static string CreateHash(string pssw, string salt)
        {
            var bajty = KeyDerivation.Pbkdf2(

                password: pssw,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
                );
            return Convert.ToBase64String(bajty);
        }

        public static string CreateSalt()
        {
            byte[] bajty = new byte[128 / 8];
            using (var generatorLiczbLosowych = RandomNumberGenerator.Create())
            {
                generatorLiczbLosowych.GetBytes(bajty);
                return Convert.ToBase64String(bajty);
            }
        }

        public static bool  CheckHash(string plaintext, string salt, string hash)
        {
            return CreateHash(plaintext,salt).Equals(hash);
        }

    }
}
