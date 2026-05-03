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

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);
		SetupGlobalHooks();
		ShowMainWindow();
		ShowSelectionWindow();
	}

	protected override void OnExit(ExitEventArgs e)
	{
		base.OnExit(e);
		Ioc.Default.GetService<IGlobalMouseHookService>()!.UnHook();
		Ioc.Default.GetService<IGlobalKeyboardHookService>()!.UnHook();
	}

	private static void SetupGlobalHooks()
	{
		Ioc.Default.GetService<IGlobalMouseHookService>()!.SetHook();
		Ioc.Default.GetService<IGlobalKeyboardHookService>()!.SetHook();
	}

	private void ShowMainWindow()
	{
		var shellWindow = new ShellWindow
		{
			DataContext = Ioc.Default.GetService<ShellWindowViewModel>()
		};
		shellWindow.Show();
		Current.MainWindow = shellWindow;
	}

	private static void ShowSelectionWindow()
	{
		var selectWindow = new SelectLaunchAppWindow
		{
			DataContext = Ioc.Default.GetService<SelectLaunchAppWindowViewModel>()
		};
		selectWindow.Show();
	}
}
