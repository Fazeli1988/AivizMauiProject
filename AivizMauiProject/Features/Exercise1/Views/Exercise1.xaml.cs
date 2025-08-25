using AivizMauiProject.Features.Exercise1.Services;
using AivizMauiProject.Features.Exercise1.ViewModels;

namespace AivizMauiProject.Features.Exercise1.Views;

public partial class Exercise1 : ContentPage
{
	public Exercise1()
	{
		InitializeComponent();
		BindingContext = new Exercise1ViewModel(new PrimeService());
	}
    private void OnClearClicked(object sender, EventArgs e)
    {
        NumberEntry.Focus(); 
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Reset or reload data
        if (BindingContext is Exercise1ViewModel vm)
        {
            vm.Load(); // A method that reloads the ViewModel's data
        }
    }
}