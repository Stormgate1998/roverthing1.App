using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MonkeyCache.FileStore;
using System.Net.Http;
using System.Threading.Tasks;

namespace roverthing1;

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
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<JoinViewModel>();
		builder.Services.AddSingleton<PlayGame>();
		builder.Services.AddSingleton<PlayGameViewModel>();
		builder.Services.AddSingleton<INavigationService, NavigationService>();
        Routing.RegisterRoute(nameof(PlayGame), typeof(PlayGame));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        Barrel.ApplicationId = "offline_sync_data";

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
