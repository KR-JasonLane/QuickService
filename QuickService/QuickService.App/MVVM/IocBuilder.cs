using QuickService.Abstract.Interfaces;
using QuickService.Core.Services;
using QuickService.ViewModels;

namespace QuickService.App;

public static class IocBuilder
{
	/// <summary>
	/// IoC 컨테이너 빌드
	/// </summary>
	public static void Build()
	{
		var services = new ServiceCollection();
		RegisterServices(services);
		RegisterViewModels(services);
		Ioc.Default.ConfigureServices(services.BuildServiceProvider());
	}

	private static void RegisterServices(IServiceCollection services)
	{
		services.AddSingleton<IJsonFileService, JsonFileService>();
		services.AddSingleton<IConfigurationService, ConfigurationService>();
		services.AddSingleton<ILaunchAppService, LaunchAppService>();
		services.AddSingleton<IGlobalMouseHookService, GlobalMouseHookService>();
		services.AddSingleton<IGlobalKeyboardHookService, GlobalKeyboardHookService>();
		services.AddSingleton<IHideMainWindowService, HideMainWindowService>();
		services.AddSingleton<ITrayIconService, TrayIconService>();
		services.AddSingleton<IUserSelectPathService, UserSelectPathService>();
		services.AddSingleton<IDialogHostService, DialogHostService>();
	}

	private static void RegisterViewModels(IServiceCollection services)
	{
		services.AddTransient<ShellWindowViewModel>();
		services.AddTransient<MainViewModel>();
		services.AddTransient<TitleViewModel>();
		services.AddTransient<InteractionViewModel>();
		services.AddTransient<SelectedFileViewModel>();
		services.AddTransient<SelectLaunchAppWindowViewModel>();
	}
}
