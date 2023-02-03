using Microsoft.Extensions.Logging;
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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<JoinViewModel>();
		builder.Services.AddSingleton<PlayGame>();
		builder.Services.AddSingleton<PlayGameViewModel>();
        Routing.RegisterRoute(nameof(PlayGame), typeof(PlayGame));

#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
