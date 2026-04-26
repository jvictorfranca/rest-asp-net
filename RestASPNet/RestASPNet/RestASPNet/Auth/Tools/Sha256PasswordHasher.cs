

using RestASPNet.Auth.Contract;
using System.Text;

namespace RestASPNet.Auth.Tools
{
    public class Sha256PasswordHasher : IPasswordHasher
    {
        public bool Verify(string password, string hashedPassword)
        {
            return Hash(password) == hashedPassword;
        }
        public string Hash(string password)
        {
            var inputBytes = Encoding.UTF8.GetBytes(password);
            var hashedBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);

            var builder = new StringBuilder();
            
            foreach (var b in hashedBytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

    }
}
