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

	#region Methods

	/// <summary>
	/// 시스템 시작
	/// </summary>
	/// <param name="e"> 이벤트 파라미터 </param>
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		////////////////////////////////////////
		// Shell Window
		////////////////////////////////////////
		{
			ShellWindow shellwindow = new() { DataContext = Ioc.Default.GetService<ShellWindowViewModel>() };

			shellwindow.ShowDialog();
		}

		////////////////////////////////////////
		// Shell Window
		////////////////////////////////////////
		{
			SelectLaunchAppWindow selectLaunchAppwindow = new() { DataContext = Ioc.Default.GetService<SelectLaunchAppWindowViewModel>() };

			selectLaunchAppwindow.Hide();
		}
	}

	#endregion
}
