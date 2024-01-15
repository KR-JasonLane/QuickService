using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class SelectLaunchAppWindowViewModel : ObservableRecipient, IViewModel
{
	public SelectLaunchAppWindowViewModel()
	{

	}

	#region Properties

	/// <summary>
	/// 좌측 앱 선택
	/// </summary>
	[ObservableProperty]
	private bool _isSelectedLeft;

	/// <summary>
	/// 상단 앱 선택
	/// </summary>
	[ObservableProperty]
	private bool _isSelectedTop;

	/// <summary>
	/// 우측 앱 선택
	/// </summary>
	[ObservableProperty]
	private bool _isSelectedRight;

	/// <summary>
	/// 하단 앱 선택
	/// </summary>
	[ObservableProperty]
	private bool _isSelectedBottom;

	/// <summary>
	/// 선택 안함
	/// </summary>
	[ObservableProperty]
	private bool _isNoneSelect;


	/// <summary>
	/// 마우스가 가리키고 있는 각도
	/// </summary>
	[ObservableProperty]
	private double _pointerAngle;


	/// <summary>
	/// 좌측 앱 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource _leftAppIconImageSorce;

	/// <summary>
	/// 상단 앱 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource _topAppIconImageSource;

	/// <summary>
	/// 우측 앱 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource _rightAppIconImageSource;

	/// <summary>
	/// 하단 앱 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource _bottomAppIconImageSource;

	#endregion

	#region Commands

	#endregion

	#region Methods

	#endregion
}
