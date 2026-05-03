using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.Extensions;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;

public partial class SelectedFileViewModel : ObservableRecipient, IViewModel
{
	private const string EmptyAppName = "Empty";
	private readonly IConfigurationService _configurationService;

	public SelectedFileViewModel(IConfigurationService configurationService)
	{
		_configurationService = configurationService;

		WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this, (r, m) =>
			UpdateAppDisplay(m.Value));

		foreach (var position in Enum.GetValues<AppPosition>())
			UpdateAppDisplay(position);
	}

	[ObservableProperty] private ImageSource? _leftAppIconImageSource;
	[ObservableProperty] private string? _leftAppName;
	[ObservableProperty] private ImageSource? _topAppIconImageSource;
	[ObservableProperty] private string? _topAppName;
	[ObservableProperty] private ImageSource? _rightAppIconImageSource;
	[ObservableProperty] private string? _rightAppName;
	[ObservableProperty] private ImageSource? _bottomAppIconImageSource;
	[ObservableProperty] private string? _bottomAppName;

	/// <summary>
	/// 지정된 위치의 앱 표시 정보를 갱신
	/// </summary>
	private void UpdateAppDisplay(AppPosition position)
	{
		var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();

		if (config is null)
			throw new InvalidOperationException("사용자 설정 불러오기 실패");

		var appInfo = config.GetByPosition(position);
		var (icon, name) = ExtractDisplayInfo(appInfo);
		ApplyDisplay(position, icon, name);
	}

	private static (ImageSource? icon, string name) ExtractDisplayInfo(AppInformationModel app)
	{
		if (!app.HasValidPath())
			return (Properties.Resources.EmptyIcon.ToImageSource(), EmptyAppName);

		app.LoadInfoFromPath();
		return (app.IconImage, app.DisplayName ?? EmptyAppName);
	}

	private void ApplyDisplay(AppPosition position, ImageSource? icon, string name)
	{
		switch (position)
		{
			case AppPosition.Left:   LeftAppIconImageSource   = icon; LeftAppName   = name; break;
			case AppPosition.Top:    TopAppIconImageSource    = icon; TopAppName    = name; break;
			case AppPosition.Right:  RightAppIconImageSource  = icon; RightAppName  = name; break;
			case AppPosition.Bottom: BottomAppIconImageSource = icon; BottomAppName = name; break;
		}
	}
}
