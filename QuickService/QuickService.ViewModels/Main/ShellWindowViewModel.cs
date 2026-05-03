using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class ShellWindowViewModel : ObservableRecipient, IViewModel
{
	public ShellWindowViewModel(MainViewModel mainViewModel)
	{
		ShellContent = mainViewModel;
	}

	/// <summary>
	/// ShellWindow에 표현할 콘텐츠
	/// </summary>
	[ObservableProperty]
	private IViewModel? _shellContent;
}
