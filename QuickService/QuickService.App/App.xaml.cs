using QuickService.Abstract.Interfaces;
using QuickService.ViewModels;
using QuickService.Views;

namespace QuickService.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public App()
	{
		IocBuilder.Build();
	}

	#region Override Methods

	/// <summary>
	/// 시스템 시작
	/// </summary>
	/// <param name="e"> 이벤트 파라미터 </param>
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		////////////////////////////////////////
		// Global Hook
		////////////////////////////////////////
		{
			Ioc.Default.GetService<IGlobalMouseHookService	 >()!.SetHook();
			Ioc.Default.GetService<IGlobalKeyboardHookService>()!.SetHook();
		}


		////////////////////////////////////////
		// Shell Window
		////////////////////////////////////////
		{
			ShellWindow shellwindow = new() { DataContext = Ioc.Default.GetService<ShellWindowViewModel>() };
			shellwindow.Show();

			Current.MainWindow = shellwindow;
		}

		////////////////////////////////////////
		// Selected Launch App Window
		////////////////////////////////////////
		{
			SelectLaunchAppWindow selectLaunchAppWindow = new() { DataContext = Ioc.Default.GetService<SelectLaunchAppWindowViewModel>() };
			selectLaunchAppWindow.Show();
		}
	}

	/// <summary>
	/// 시스템 종료
	/// </summary>
	/// <param name="e"> 이벤트 파라미터 </param>
	protected override void OnExit(ExitEventArgs e)
	{
		base.OnExit(e);

		////////////////////////////////////////
		// Global Hook
		////////////////////////////////////////
		{
			Ioc.Default.GetService<IGlobalMouseHookService	 >()!.UnHook();
			Ioc.Default.GetService<IGlobalKeyboardHookService>()!.UnHook();
		}
	}

	#endregion
}
