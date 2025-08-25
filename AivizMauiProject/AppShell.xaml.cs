using AivizMauiProject.Features.Home;
using AivizMauiProject.Features.Exercise1.Views;
using AivizMauiProject.Features.Exercise2.Views;
using AivizMauiProject.Features.Exercise3.Views;

namespace AivizMauiProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // routes for pages
            Routing.RegisterRoute("exercise1", typeof(Exercise1));
            Routing.RegisterRoute("exercise2", typeof(Exercise2));
            Routing.RegisterRoute("exercise3", typeof(Exercise3));
        }

        private async void OnExercise1Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("exercise1");
        }

        private async void OnExercise2Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("exercise2");
        }

        private async void OnExercise3Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("exercise3");
        }

        private void OnExitClicked(object sender, EventArgs e)
        {
            #if ANDROID
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            #elif WINDOWS
                if (Application.Current != null)
                    Application.Current.Quit();
            #else
                System.Environment.Exit(0);
            #endif
        }
    }
}
