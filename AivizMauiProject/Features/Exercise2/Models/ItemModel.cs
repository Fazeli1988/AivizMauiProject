using AivizMauiProject.Common.Base;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AivizMauiProject.Features.Exercise2.Models
{
    public class ItemModel : BaseViewModel
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private DateTime _date = DateTime.Now;
        private string _image = string.Empty;
        private bool _isSelected;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        // For compatibility with legacy code:
        public void RaisePropertyChanged(string propertyName) => OnPropertyChanged(propertyName);
    }
}
