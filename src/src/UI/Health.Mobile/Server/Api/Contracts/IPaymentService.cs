namespace Health.Mobile.Server.Api.Contracts;

public interface IPaymentService
{
    Task<string?> PaymentByTeleBirr(double amount, string userName);
}