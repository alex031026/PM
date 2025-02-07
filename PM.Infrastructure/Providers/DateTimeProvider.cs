using PM.Application.Common.Providers;

namespace PM.Infrastructure.Providers;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}