using AivizMauiProject.Features.Exercise2.Services;
using AivizMauiProject.Features.Exercise2.ViewModels;
using Microsoft.Extensions.Logging;

namespace AivizMauiProject
{
    public static class MauiProgram 
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-regular-400.ttf", "FARegular");
                });

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddSingleton<IItemService, ItemService>();
            builder.Services.AddTransient<Exercise2ViewModel>();
#endif

            return builder.Build();
        }
    }
}
