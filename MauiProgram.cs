using MobileAppMAUI.Services;
using Microsoft.Extensions.Logging;

namespace MobileAppMAUI
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
                    fonts.AddFont("PressStart2P-Regular.ttf", "Pixel");
                });

            #if DEBUG
                builder.Logging.AddDebug();
            #endif
            builder.Services.AddSingleton<DataService>();
            return builder.Build();
        }
    }
}

