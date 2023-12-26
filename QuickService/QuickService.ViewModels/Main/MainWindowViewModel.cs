using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient, IViewModel
{
	public MainWindowViewModel(IViewModel mainViewModel)
	{
		////////////////////////////////////////
		// 뷰모델 기본 속성
		////////////////////////////////////////
		{

		}


		////////////////////////////////////////
		// 바인딩 속성
		////////////////////////////////////////
		{

		}


		////////////////////////////////////////
		// 뷰모델 의존성
		////////////////////////////////////////
		{
			ShellContent = mainViewModel;
		}
	}

	#region Properties

	/// <summary>
	/// MainWindow에 표현되는 MainView
	/// </summary>
	[ObservableProperty]
	public object? _shellContent;

	#endregion

	#region Methods

	#endregion
}
