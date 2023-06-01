namespace RazorShared.Server.Api.Contracts;

public interface IPaymentService
{
    Task<string?> PaymentByTeleBirr(double amount);
}