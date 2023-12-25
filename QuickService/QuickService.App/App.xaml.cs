using QuickService.Abstract.Interfaces;

using QuickService.ViewModels.Factory;
using QuickService.ViewModels.Window;

namespace QuickService.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	public App()
	{
		////////////////////////////////////////
		// 뷰모델 의존성 주입
		////////////////////////////////////////
		{
			IServiceCollection collection = new ServiceCollection();
			collection.AddKeyedTransient<IViewModel, MainWindowViewModel>(nameof(MainWindowViewModel));
			collection.AddSingleton<ViewModelFactory>();
			Container = collection.BuildServiceProvider();
		}
	}

	#region Properties

	public IServiceProvider Container { get; init; }

	#endregion

	#region Methods

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		////////////////////////////////////////
		// 팩토리 매서드
		////////////////////////////////////////
		{
			ViewModelFactory factory = Container.GetRequiredService<ViewModelFactory>();
			MainWindow window = new()
			{
				DataContext = factory.GetMainWindowViewModel()
			};
			window.ShowDialog();
		}
	}

	#endregion
}
