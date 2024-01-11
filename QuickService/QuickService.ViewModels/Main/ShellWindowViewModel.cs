using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;
public partial class ShellWindowViewModel : ObservableRecipient, IViewModel
{
	public ShellWindowViewModel()
	{
		////////////////////////////////////////
		// 바인딩 속성
		////////////////////////////////////////
		{
			ShellContent = Ioc.Default.GetService<MainViewModel>()!;
		}
	}

	#region Properties

	/// <summary>
	/// ShellWindow에 표현
	/// </summary>
	[ObservableProperty]
	public IViewModel? _shellContent;

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion
}
