namespace PM.Application.Common.Providers;
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
