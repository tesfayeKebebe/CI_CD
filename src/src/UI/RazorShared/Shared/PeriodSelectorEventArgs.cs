using RazorShared.Server.Enums;

namespace RazorShared.Shared;

public class PeriodSelectorEventArgs
{
    public PeriodSelectorEventArgs(Period period, DateTime? from, DateTime? until)
    {
        Period = period;
        From = from;
        Until = until;
    }

    public Period Period { get; }
    public DateTime? From { get; }
    public DateTime? Until { get; }
}