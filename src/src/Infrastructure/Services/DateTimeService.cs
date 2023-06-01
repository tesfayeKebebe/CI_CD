using Application.Common.Interfaces;
using Application.Interfaces;

namespace Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow.AddHours(3);
}
