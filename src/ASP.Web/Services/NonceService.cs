using System.Security.Cryptography;

namespace ASP.Web.Services
{
    public class NonceService : INonceService
    {
        private readonly string _nonce;

        public NonceService(int nonceByteAmount = 32)
        {
            byte[] nonceBytes = new byte[nonceByteAmount];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nonceBytes);
            }

            _nonce = Convert.ToBase64String(nonceBytes);
        }

        public string GetNonce()
        {
            return _nonce;
        }
    }
}

