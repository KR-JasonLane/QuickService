using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class MainViewModel : ObservableRecipient, IViewModel
{
	public MainViewModel(
		TitleViewModel titleViewModel,
		InteractionViewModel interactionViewModel,
		SelectedFileViewModel selectedFileViewModel)
	{
		TitleContent        = titleViewModel;
		InteractionContent  = interactionViewModel;
		SelectedFileContent = selectedFileViewModel;
	}

	/// <summary>
	/// 타이틀 뷰모델
	/// </summary>
	[ObservableProperty]
	private IViewModel _titleContent;

	/// <summary>
	/// 상호작용 뷰모델
	/// </summary>
	[ObservableProperty]
	private IViewModel _interactionContent;

	/// <summary>
	/// 선택된 파일표시 뷰모델
	/// </summary>
	[ObservableProperty]
	private IViewModel _selectedFileContent;
}
