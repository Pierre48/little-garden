using System.Security.Cryptography;
using System.Text;

namespace LittleGarden.Core.Entities
{
    public abstract class Entity
    {
        private static readonly HashAlgorithm algorithm = SHA256.Create();

        public abstract byte[] _id { get; set; }

        protected static byte[] HashToInt(string value)
        {
            if (value == null) return new byte[] {0};
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            return hash;
        }
    }
}