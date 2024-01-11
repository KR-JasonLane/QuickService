using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;
public partial class MainViewModel : ObservableRecipient, IViewModel
{
	public MainViewModel()
	{
		////////////////////////////////////////
		// 바인딩 속성
		////////////////////////////////////////
		{
			TitleContent		= Ioc.Default.GetService<TitleViewModel>		()!;
			InteractionContent	= Ioc.Default.GetService<InteractionViewModel>	()!;
			SelectedFileContent = Ioc.Default.GetService<SelectedFileViewModel>	()!;
		}
	}

	#region Properties

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

    #endregion

    #region Commands

    #endregion

    #region Methods

    #endregion
}
