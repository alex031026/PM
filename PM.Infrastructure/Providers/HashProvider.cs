using PM.Application.Common.Providers;
using System.Security.Cryptography;
using System.Text;

namespace PM.Infrastructure.Providers;
public class HashProvider : IHashProvider
{
    public string GetHash(string value) => Convert.ToHexString(SHA1.HashData(Encoding.UTF8.GetBytes(value)));
}
