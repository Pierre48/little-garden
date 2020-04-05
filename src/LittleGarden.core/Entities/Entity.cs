using System.Security.Cryptography;
using System.Text;

namespace LittleGarden.Core.Entities
{
 
        public abstract class Entity
        {
            static HashAlgorithm algorithm = SHA256.Create();

            protected static byte[] HashToInt(string value)
            {
                if (value == null) return new byte[] { 0 };
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
                return hash;
            }

            public abstract byte[] _id { get; set; }
        }
}