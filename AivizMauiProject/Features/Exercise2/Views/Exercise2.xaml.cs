using AivizMauiProject.Features.Exercise2.ViewModels;

namespace AivizMauiProject.Features.Exercise2.Views
{
    public partial class Exercise2 : ContentPage
    {
        public Exercise2(Exercise2ViewModel viewModel)
        {
            InitializeComponent();
            SizeChanged += OnPageSizeChanged;
            BindingContext = viewModel;

        }

        private void OnPageSizeChanged(object sender, EventArgs e)
        {
            // Calculating the number of columns based on the page width
            int span = (int)(Width / 180);
            if (span < 1) span = 1;
            GridLayout.Span = span;
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (BindingContext is not Exercise2ViewModel vm || vm.SelectedItem == null)
                return;

            bool confirm = await DisplayAlert("Confirm Delete",
                "Are you sure you want to delete this item?",
                "Yes", "No");

            if (confirm)
                vm.DeleteSelectedItem();
        }
    }
}