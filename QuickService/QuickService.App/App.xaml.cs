
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
		////////////////////////////////////////
		// Dependency
		////////////////////////////////////////
		{
			var services = ConfigureServices();
			Ioc.Default.ConfigureServices(services);
		}
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
			ShellWindow window = new() { DataContext = Ioc.Default.GetService<ShellWindowViewModel>() };			

			window.ShowDialog();
		}
	}

	/// <summary>
	/// Ioc컨테이너 생성
	/// </summary>
	/// <returns> IServiceProvider 객체 </returns>
	private IServiceProvider ConfigureServices()
	{
		var services = new ServiceCollection();

		services.AddTransient<ShellWindowViewModel>();
		services.AddTransient<MainViewModel>();
		services.AddTransient<TitleViewModel>();
		services.AddTransient<InteractionViewModel>();
		services.AddTransient<SelectedFileViewModel>();

		return services.BuildServiceProvider();
	}

	#endregion
}
