using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Claims;
using System.Security.Principal;

namespace Upload.App.Utils
{
    public static class Utils
    {
        public static string PassGenerate(string password)
        {
            byte[] salt = BitConverter.GetBytes(28379535); 

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static string GetUserName(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Name);
            return claim == null ? null : claim.Value;
        }

        public static string GetUserId(this IPrincipal user)
        {
            var claim = ((ClaimsIdentity)user.Identity).FindFirst(ClaimTypes.Sid);
            return claim == null ? null : claim.Value;
        }
    }
}