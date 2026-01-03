using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.Extensions;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;
public partial class SelectedFileViewModel : ObservableRecipient, IViewModel
{
    public SelectedFileViewModel(IConfigurationService configurationService)
    {
		////////////////////////////////////////
		// 메신저 등록
		////////////////////////////////////////
		{
			WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this, (r, m) =>
			{
				LoadUserSelectedAppIconImages(m.Value);
			});
		}


		////////////////////////////////////////
		// 서비스 등록
		////////////////////////////////////////
		{
            _configurationService = configurationService;
        }


		////////////////////////////////////////
		// 사용자 설정 로드
		////////////////////////////////////////
		{
            LoadUserSelectedAppIconImages(AppPosition.Left	);
			LoadUserSelectedAppIconImages(AppPosition.Right	);
			LoadUserSelectedAppIconImages(AppPosition.Top	);
			LoadUserSelectedAppIconImages(AppPosition.Bottom);
		}
	}

	#region Services

	private readonly IConfigurationService _configurationService;

    #endregion

    #region Properties

	/// <summary>
	/// 좌측 어플리케이션 아이콘 이미지
	/// </summary>
    [ObservableProperty]
    private ImageSource? _leftAppIconImageSource;

	/// <summary>
	/// 좌측 어플리케이션 이름
	/// </summary>
    [ObservableProperty]
    private string? _leftAppName;

	/// <summary>
	/// 상단 어플리케이션 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource? _topAppIconImageSource;

	/// <summary>
	/// 상단 어플리케이션 이름
	/// </summary>
	[ObservableProperty]
	private string? _topAppName;

	/// <summary>
	/// 우측 어플리케이션 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource? _rightAppIconImageSource;

	/// <summary>
	/// 우측 어플리케이션 이름
	/// </summary>
	[ObservableProperty]
	private string? _rightAppName;

	/// <summary>
	/// 하단 어플리케이션 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource? _bottomAppIconImageSource;

	/// <summary>
	/// 하단 어플리케이션 이름
	/// </summary>
	[ObservableProperty]
	private string? _bottomAppName;

	#endregion

	#region Methods

	/// <summary>
	/// 선택된 앱들의 정보를 로드
	/// </summary>
	private void LoadUserSelectedAppIconImages(AppPosition position)
    {
        var configuration = _configurationService.GetConfiguration<RegisteredApplicationModel>();

        if(configuration is null)
			throw new InvalidOperationException("Error : 사용자 설정 불러오기 실패!");

		var appInfo = configuration.GetByPosition(position);

		if (appInfo.HasValidPath())
		{
			appInfo.LoadInfoFromPath();
			ApplyAppDisplay(position, appInfo.IconImage!, appInfo.DisplayName!);
		}
		else
		{
			ApplyAppDisplay(position, Properties.Resources.EmptyIcon.ToImageSource()!, "Empty");
		}
    }

	/// <summary>
	/// 위치에 해당하는 앱 표시 정보를 적용
	/// </summary>
	private void ApplyAppDisplay(AppPosition position, ImageSource icon, string name)
	{
		switch (position)
		{
			case AppPosition.Left:   LeftAppIconImageSource   = icon; LeftAppName   = name; break;
			case AppPosition.Top:    TopAppIconImageSource    = icon; TopAppName    = name; break;
			case AppPosition.Right:  RightAppIconImageSource  = icon; RightAppName  = name; break;
			case AppPosition.Bottom: BottomAppIconImageSource = icon; BottomAppName = name; break;
		}
	}

    #endregion
}
