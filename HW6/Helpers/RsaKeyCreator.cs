using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace HW6.Helpers
{
    internal static class RsaKeyCreator
    {
        public static RsaSecurityKey GetPublicSecurityKey(string publicRsaKey)
        {
            var publicKeyBytes = Convert.FromBase64String(publicRsaKey);
            var publicCrypto = new RSACryptoServiceProvider(1024);
            publicCrypto.ImportRSAPublicKey(publicKeyBytes, out _);
            return new RsaSecurityKey(publicCrypto);
        }
    }
}
