using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.Domain.Extensions;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;
public partial class SelectedFileViewModel : ObservableRecipient, IViewModel
{
    public SelectedFileViewModel(IConfigurationService configurationService)
    {
		////////////////////////////////////////
		// 사용자 설정 로드
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
            LoadUserSelectedAppIconImages(AppPosition.LEFT	);
			LoadUserSelectedAppIconImages(AppPosition.RIGHT	);
			LoadUserSelectedAppIconImages(AppPosition.TOP	);
			LoadUserSelectedAppIconImages(AppPosition.BOTTOM);
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

        if(configuration != null)
		{
			switch (position)
			{
				case AppPosition.LEFT:
					if (configuration.LeftAppInformation.IsValidPath())
					{
						LeftAppIconImageSource = configuration.LeftAppInformation.GetIconImage()!;
						LeftAppName			   = configuration.LeftAppInformation.GetAppName  ()!;
					}
					else
					{
						LeftAppIconImageSource = Properties.Resources.EmptyIcon.ToImageSource ()!;
						LeftAppName			   = "Empty";
					}
				break;

				case AppPosition.TOP:
					if(configuration.TopAppInformation.IsValidPath())
					{
						TopAppIconImageSource = configuration.TopAppInformation.GetIconImage()!;
						TopAppName			  = configuration.TopAppInformation.GetAppName  ()!;
					}
					else
					{
						TopAppIconImageSource = Properties.Resources.EmptyIcon.ToImageSource()!;
						TopAppName			  = "Empty";
					}
				break; 

				case AppPosition.RIGHT:
					if (configuration.RightAppInformation.IsValidPath())
					{
						RightAppIconImageSource = configuration.RightAppInformation.GetIconImage()!;
						RightAppName			= configuration.RightAppInformation.GetAppName  ()!;
					}
					else
					{
						RightAppIconImageSource = Properties.Resources.EmptyIcon.ToImageSource  ()!;
						RightAppName			= "Empty";
					}
				break;

				case AppPosition.BOTTOM:
					if (configuration.BottomAppInformation.IsValidPath())
					{
						BottomAppIconImageSource = configuration.BottomAppInformation.GetIconImage()!;
						BottomAppName			 = configuration.BottomAppInformation.GetAppName  ()!;
					}
					else
					{
						BottomAppIconImageSource = Properties.Resources.EmptyIcon.ToImageSource   ()!;
						BottomAppName			 = "Empty";
					}
				break;
			}
		}
		else
		{
			throw new InvalidOperationException("Error : 사용자 설정 불러오기 실패! ");
		}
    }

    #endregion
}
