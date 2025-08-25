using AivizMauiProject.Common.Enums;


namespace AivizMauiProject.Features.Exercise1.Services
{
    public interface IPrimeService
    {
        (bool isValid, bool isPrime, string message, PrimeResultStatus status) CheckPrime(string input);
    }
}
