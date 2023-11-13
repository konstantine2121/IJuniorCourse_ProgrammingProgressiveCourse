using System;
using System.Security.Cryptography;
using System.Text;

namespace _27_Task.Utils
{
    public sealed class HashCalculator
    {
        private readonly HashAlgorithm _hashAlgorithm;

        private HashCalculator(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
        }

        public static HashCalculator CreateSha256Calculator() => new HashCalculator(SHA256.Create());

        public string CalculateHash(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var hash = _hashAlgorithm.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
