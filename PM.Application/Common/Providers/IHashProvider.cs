namespace PM.Application.Common.Providers;

public interface IHashProvider
{
    string GetHash(string value);
}
