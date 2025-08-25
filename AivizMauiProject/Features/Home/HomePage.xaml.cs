namespace AivizMauiProject.Features.Home;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomeViewModel();
    }

    private async void GoToExercise1(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("exercise1");
    }

    private async void GoToExercise2(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("exercise2");
    }

    private async void GoToExercise3(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("exercise3");
    }
}