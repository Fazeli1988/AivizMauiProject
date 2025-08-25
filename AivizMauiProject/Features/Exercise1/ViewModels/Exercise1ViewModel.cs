using AivizMauiProject.Common.Base;
using AivizMauiProject.Common.Enums;
using AivizMauiProject.Features.Exercise1.Services;
using System.Windows.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AivizMauiProject.Features.Exercise1.ViewModels
{
    public class Exercise1ViewModel : BaseViewModel
    {
        private readonly IPrimeService _primeService;

        private string _numberInput= string.Empty;
        private string _resultMessage= string.Empty;
        private PrimeResultStatus _resultStatus = PrimeResultStatus.None;

        public string NumberInput
        {
            get => _numberInput;
            set { _numberInput = value; OnPropertyChanged(); }
        }

        public string ResultMessage
        {
            get => _resultMessage;
            set { _resultMessage = value; OnPropertyChanged(); }
        }

        public PrimeResultStatus ResultStatus
        {
            get => _resultStatus;
            set { _resultStatus = value; OnPropertyChanged(); }
        }

        public ICommand CheckPrimeCommand { get; }

        public Exercise1ViewModel(IPrimeService primeService)
        {
            _primeService = primeService;
            ClearCommand = new Command(() =>
            {
                NumberInput = string.Empty;
                ResultMessage = string.Empty;

            });
            CheckPrimeCommand = new Command(CheckPrime);
        }

        public ICommand ClearCommand { get; }

        private void CheckPrime()
        {
            var result = _primeService.CheckPrime(NumberInput);
            ResultMessage = result.message;
            ResultStatus = result.status;
        }


        public void Load()
        {
            // Anything that needs to be reset or reloaded

            ResultMessage = string.Empty;
            NumberInput = string.Empty;
        }

    }
}
