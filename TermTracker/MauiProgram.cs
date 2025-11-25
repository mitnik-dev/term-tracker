using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace TermTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter_18pt-Regular.ttf", "InterRegular18");
                    fonts.AddFont("Inter_18pt-Medium.ttf", "InterMedium18");
                    fonts.AddFont("Inter_24pt-Medium.ttf", "InterMedium24");
                    fonts.AddFont("Inter_28pt-SemiBold.ttf", "InterSemiBold28");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
