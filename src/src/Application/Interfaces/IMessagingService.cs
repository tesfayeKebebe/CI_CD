namespace Application.Interfaces;

public interface IMessagingService
{
    Task<(bool, string)> SendMessage(string receiptNumber);
}