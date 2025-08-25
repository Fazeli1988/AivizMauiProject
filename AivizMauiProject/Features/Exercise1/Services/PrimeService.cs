using AivizMauiProject.Common.Enums;


namespace AivizMauiProject.Features.Exercise1.Services
{
    public class PrimeService : IPrimeService
    {
        public (bool isValid, bool isPrime, string message, PrimeResultStatus status) CheckPrime(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (false, false, "Please enter a number.", PrimeResultStatus.Error);

            if (!long.TryParse(input, out long number))
                return (false, false, "Invalid data!", PrimeResultStatus.Error);

            if (number < 2)
                return (true, false, $"{number} is not prime.", PrimeResultStatus.Error);

            bool isPrime = true;
            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                {
                    isPrime = false;
                    break;
                }
            }

            return (true, isPrime,
                isPrime ? $"{number} is prime!" : $"{number} is not prime.",
                isPrime ? PrimeResultStatus.Success : PrimeResultStatus.Error);
        }
    }
}
