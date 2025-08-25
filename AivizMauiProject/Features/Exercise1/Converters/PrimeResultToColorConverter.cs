using AivizMauiProject.Common.Enums;
using System.Globalization;

namespace AivizMauiProject.Features.Exercise1.Converters
{
    public class PrimeResultToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is PrimeResultStatus status)
            {
                return status switch
                {
                    PrimeResultStatus.Success => Colors.Green,
                    PrimeResultStatus.Error => Colors.Red,
                    _ => Colors.White
                };
            }
            return Colors.White;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
