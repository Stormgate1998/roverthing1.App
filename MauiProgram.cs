using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MonkeyCache.FileStore;
using roverthing1.Classes;
using roverthing1.ViewModels;
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
		builder.Services.AddSingleton<DronePage>();
		builder.Services.AddSingleton<DroneViewModel>();
		builder.Services.AddSingleton<RoverAPIService>();
		builder.Services.AddSingleton<FullMap>();
		builder.Services.AddSingleton<FullMapViewModel>();
		builder.Services.AddSingleton<StatisticsPageViewModel>();
		builder.Services.AddSingleton<StatisticsPage>();
        Routing.RegisterRoute(nameof(PlayGame), typeof(PlayGame));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(FullMap), typeof(FullMap));
        Routing.RegisterRoute(nameof(StatisticsPage), typeof(StatisticsPage));
		Routing.RegisterRoute(nameof(DronePage), typeof(DronePage));
        Barrel.ApplicationId = "offline_sync_data";

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
