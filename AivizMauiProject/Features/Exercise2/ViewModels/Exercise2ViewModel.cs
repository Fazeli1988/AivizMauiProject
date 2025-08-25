using AivizMauiProject.Common.Base;
using AivizMauiProject.Features.Exercise2.Models;
using AivizMauiProject.Features.Exercise2.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AivizMauiProject.Features.Exercise2.ViewModels
{
    public class Exercise2ViewModel : BaseViewModel
    {
        private readonly IItemService _itemService;

        public ObservableCollection<ItemModel> Items { get; } = new();
        private ObservableCollection<ItemModel> _allItems = new();

        private ItemModel _selectedItem;
        public ItemModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;

                if (_selectedItem != null) _selectedItem.IsSelected = false;
                _selectedItem = value;
                if (_selectedItem != null) _selectedItem.IsSelected = true;

                OnPropertyChanged();
            }
        }

        public ICommand OpenDetailsCommand { get; }

        private bool _isReadOnlyMode;
        public bool IsReadOnlyMode
        {
            get => _isReadOnlyMode;
            set => SetProperty(ref _isReadOnlyMode, value);
        }

        private ItemModel _currentItem;
        public ItemModel CurrentItem
        {
            get => _currentItem;
            set => SetProperty(ref _currentItem, value);
        }

        private bool _isPopupVisible;
        public bool IsPopupVisible
        {
            get => _isPopupVisible;
            set => SetProperty(ref _isPopupVisible, value);
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    ApplyFilter();
            }
        }

        private bool _isImagePopupVisible;
        public bool IsImagePopupVisible
        {
            get => _isImagePopupVisible;
            set => SetProperty(ref _isImagePopupVisible, value);
        }

        private string _popupImageSource;
        public string PopupImageSource
        {
            get => _popupImageSource;
            set => SetProperty(ref _popupImageSource, value);
        }

        public ICommand AddNewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand SaveItemCommand { get; }
        public ICommand PickImageCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand CancelImagePopupCommand { get; }
        public ICommand ItemTappedCommand { get; }
        public ICommand ImageDoubleTappedCommand { get; }

        // This constructor is used if dependency injection (DI) is not configured
        public Exercise2ViewModel() : this(new ItemService()) { }

        // Constructor for dependency injection
        public Exercise2ViewModel(IItemService itemService)
        {
            _itemService = itemService;

            AddNewCommand = new Command(OnAddNew);
            EditCommand = new Command(OnEdit, () => SelectedItem != null);
            SaveItemCommand = new Command(OnSaveItem);
            PickImageCommand = new Command(OnPickImage);
            CancelCommand = new Command(OnCancel);
            ItemTappedCommand = new Command<ItemModel>(item => SelectedItem = item);
            OpenDetailsCommand = new Command(OnDetail, () => SelectedItem != null);
            ImageDoubleTappedCommand = new Command(OnImageDoubleTapped);
            CancelImagePopupCommand = new Command(OnCancelImagePopup);

            LoadData();
        }

        private void OnImageDoubleTapped()
        {
            if (CurrentItem?.Image != null)
            {
                PopupImageSource = CurrentItem.Image;
                IsImagePopupVisible = true;
            }
        }

        private void LoadData()
        {
            _allItems = new ObservableCollection<ItemModel>(_itemService.GetAll());
            ApplyFilter();
        }

        private void OnCancelImagePopup()
        {
            IsImagePopupVisible = false;
            PopupImageSource = null;
        }

        private void ApplyFilter()
        {
            Items.Clear();

            var filtered = string.IsNullOrWhiteSpace(SearchText)
                ? _allItems
                : new ObservableCollection<ItemModel>(
                    _allItems.Where(i =>
                        (!string.IsNullOrEmpty(i.Title) && i.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(i.Description) && i.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)))
                  );

            foreach (var item in filtered)
                Items.Add(item);
        }

        private void OnAddNew()
        {
            CurrentItem = new ItemModel { Date = DateTime.Now };
            IsReadOnlyMode = false;
            IsPopupVisible = true;
        }

        private void OnDetail()
        {
            if (SelectedItem == null)
                return;

            CurrentItem = new ItemModel
            {
                Title = SelectedItem.Title,
                Description = SelectedItem.Description,
                Date = SelectedItem.Date,
                Image = SelectedItem.Image
            };

            IsReadOnlyMode = true;
            IsPopupVisible = true;
        }

        private void OnEdit()
        {
            if (SelectedItem == null) return;

            // Edit a copy so that the original item doesn't get affected until save is confirmed
            CurrentItem = new ItemModel
            {
                Title = SelectedItem.Title,
                Description = SelectedItem.Description,
                Date = SelectedItem.Date,
                Image = SelectedItem.Image,
                IsSelected = SelectedItem.IsSelected
            };

            IsReadOnlyMode = false;
            IsPopupVisible = true;
        }

        private async void OnPickImage()
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Select an image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null && CurrentItem != null)
            {
                CurrentItem.Image = result.FullPath;
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        private async void OnSaveItem()
        {
            if (CurrentItem == null) return;

            if (string.IsNullOrWhiteSpace(CurrentItem.Title) ||
                string.IsNullOrWhiteSpace(CurrentItem.Description))
            {
                await Application.Current.MainPage.DisplayAlert("Validation Error",
                    "Title and Description are required.", "OK");
                return;
            }

            if (SelectedItem == null)
            {
                // Add new item
                _allItems.Add(CurrentItem);
                _itemService.Add(CurrentItem);
            }
            else
            {
                // Apply changes to the selected item
                SelectedItem.Title = CurrentItem.Title;
                SelectedItem.Description = CurrentItem.Description;
                SelectedItem.Date = CurrentItem.Date;
                SelectedItem.Image = CurrentItem.Image;

                _itemService.Update(SelectedItem);

                SelectedItem.RaisePropertyChanged(nameof(SelectedItem.Title));
                SelectedItem.RaisePropertyChanged(nameof(SelectedItem.Description));
                SelectedItem.RaisePropertyChanged(nameof(SelectedItem.Date));
                SelectedItem.RaisePropertyChanged(nameof(SelectedItem.Image));
            }

            IsPopupVisible = false;
            ApplyFilter();
        }

        private void OnCancel()
        {
            CurrentItem = null;
            IsPopupVisible = false;
        }

        public void DeleteSelectedItem()
        {
            if (SelectedItem == null) return;

            _allItems.Remove(SelectedItem);
            Items.Remove(SelectedItem);
            _itemService.Delete(SelectedItem);

            SelectedItem = null;
        }
    }
}
