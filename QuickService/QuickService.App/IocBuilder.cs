﻿using QuickService.Domain.Services;
using QuickService.ViewModels;

namespace QuickService.App;

public static class IocBuilder
{

	/// <summary>
	/// IoC 컨테이너 빌드
	/// </summary>
	public static void Build()
	{
		var services = ConfigureService();

		Ioc.Default.ConfigureServices(services);
	}

	/// <summary>
	/// Service Provider 설정
	/// </summary>
	/// <returns></returns>
	private static IServiceProvider ConfigureService()
	{
		var services = new ServiceCollection();

		////////////////////////////////////////
		// Services
		////////////////////////////////////////
		{
			services.AddSingleton<IHideMainWindowService,	HideMainWindowService	>();
			services.AddSingleton<ITrayIconService		,	TrayIconService			>();
			services.AddSingleton<IUserSelectPathService,	UserSelectPathService	>();
		}


		////////////////////////////////////////
		// Shell Window
		////////////////////////////////////////
		{
			services.AddTransient<ShellWindowViewModel>();
		}


		////////////////////////////////////////
		// Shell Contents
		////////////////////////////////////////
		{
			services.AddTransient<MainViewModel			>();
			services.AddTransient<TitleViewModel		>();
			services.AddTransient<InteractionViewModel	>();
			services.AddTransient<SelectedFileViewModel	>();
		}

		return services.BuildServiceProvider();
	}
}
