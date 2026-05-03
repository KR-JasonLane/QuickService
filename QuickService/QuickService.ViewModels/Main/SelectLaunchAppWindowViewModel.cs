using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels.Messenger;
using QuickService.ViewModels.Messenger.Parameters;

namespace QuickService.ViewModels;

public partial class SelectLaunchAppWindowViewModel : ObservableRecipient, IViewModel
{
	private const double SelectionWindowSize = 300;
	private const double InvalidAreaSize     = 52;
	private const double RightTopBoundary    = 45;
	private const double TopLeftBoundary     = 135;
	private const double LeftBottomBoundary  = 225;
	private const double BottomRightBoundary = 315;

	private readonly IConfigurationService _configurationService;
	private readonly ILaunchAppService _launchAppService;
	private Point _currentWindowCenter;

	public SelectLaunchAppWindowViewModel(
		IConfigurationService configurationService,
		ILaunchAppService launchAppService)
	{
		_configurationService = configurationService;
		_launchAppService     = launchAppService;

		WindowLength        = SelectionWindowSize;
		InvalidAreaDiameter = InvalidAreaSize;

		RegisterMessengers();
		LoadAllAppIcons();
	}

	#region Observable Properties

	[ObservableProperty] private bool _isSelectedLeft;
	[ObservableProperty] private bool _isSelectedTop;
	[ObservableProperty] private bool _isSelectedRight;
	[ObservableProperty] private bool _isSelectedBottom;
	[ObservableProperty] private bool _isNoneSelect;
	[ObservableProperty] private double _pointerAngle;
	[ObservableProperty] private bool _isWindowOpen;
	[ObservableProperty] private ImageSource? _leftAppIconImageSource;
	[ObservableProperty] private ImageSource? _topAppIconImageSource;
	[ObservableProperty] private ImageSource? _rightAppIconImageSource;
	[ObservableProperty] private ImageSource? _bottomAppIconImageSource;
	[ObservableProperty] private double _positionLeft;
	[ObservableProperty] private double _positionTop;
	[ObservableProperty] private double _windowLength;
	[ObservableProperty] private double _invalidAreaDiameter;

	#endregion

	private void RegisterMessengers()
	{
		WeakReferenceMessenger.Default.Register<MouseClickStateMessage>(this, (r, m) =>
			HandleMouseClick(m.Value));
		WeakReferenceMessenger.Default.Register<ModifierKeyStateMessage>(this, (r, m) =>
		{
			if (!m.Value) IsWindowOpen = false;
		});
		WeakReferenceMessenger.Default.Register<MouseMoveMessage>(this, (r, m) =>
		{
			if (IsWindowOpen) HandleMouseMove(new Point(m.Value.X, m.Value.Y));
		});
		WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this, (r, m) =>
			LoadAppIcon(m.Value));
	}

	private void HandleMouseClick(MouseClickParameter click)
	{
		IsWindowOpen = click.IsDown;
		IsNoneSelect = true;

		if (click.IsDown)
		{
			PositionLeft = click.X - (WindowLength / 2);
			PositionTop  = click.Y - (WindowLength / 2);
			_currentWindowCenter = new Point(click.X, click.Y);
			HandleMouseMove(_currentWindowCenter);
		}
		else
		{
			LaunchSelectedApp();
		}
	}

	private void HandleMouseMove(Point mousePosition)
	{
		if (IsWithinInvalidArea(mousePosition))
		{
			ClearSelection();
			IsNoneSelect = true;
			return;
		}

		IsNoneSelect = false;
		var angle = CalculateAngle(mousePosition);
		PointerAngle = -angle + 90;
		ApplySelection(DeterminePosition(angle));
	}

	private bool IsWithinInvalidArea(Point mouse)
	{
		var radius = InvalidAreaDiameter / 2;
		var dx = (mouse.X - _currentWindowCenter.X) / radius;
		var dy = (mouse.Y - _currentWindowCenter.Y) / radius;
		return (dx * dx + dy * dy) <= 1.0;
	}

	private double CalculateAngle(Point mouse)
	{
		var deltaX = mouse.X - _currentWindowCenter.X;
		var deltaY = _currentWindowCenter.Y - mouse.Y;
		return Math.Atan2(deltaY, deltaX) * (180 / Math.PI);
	}

	private static AppPosition DeterminePosition(double angle)
	{
		if (angle < 0) angle += 360;
		return angle switch
		{
			>= BottomRightBoundary or < RightTopBoundary => AppPosition.Right,
			>= RightTopBoundary and < TopLeftBoundary    => AppPosition.Top,
			>= TopLeftBoundary and < LeftBottomBoundary  => AppPosition.Left,
			_                                            => AppPosition.Bottom,
		};
	}

	private void ApplySelection(AppPosition position)
	{
		ClearSelection();
		switch (position)
		{
			case AppPosition.Left:   IsSelectedLeft   = true; break;
			case AppPosition.Top:    IsSelectedTop    = true; break;
			case AppPosition.Right:  IsSelectedRight  = true; break;
			case AppPosition.Bottom: IsSelectedBottom = true; break;
		}
	}

	private void ClearSelection()
	{
		IsSelectedLeft = IsSelectedTop = IsSelectedRight = IsSelectedBottom = false;
	}

	private void LoadAllAppIcons()
	{
		foreach (var position in Enum.GetValues<AppPosition>())
			LoadAppIcon(position);
	}

	private void LoadAppIcon(AppPosition position)
	{
		var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
		if (config is null) return;

		var appInfo = config.GetByPosition(position);
		if (!appInfo.HasValidPath()) return;

		appInfo.LoadInfoFromPath();
		switch (position)
		{
			case AppPosition.Left:   LeftAppIconImageSource   = appInfo.IconImage; break;
			case AppPosition.Top:    TopAppIconImageSource    = appInfo.IconImage; break;
			case AppPosition.Right:  RightAppIconImageSource  = appInfo.IconImage; break;
			case AppPosition.Bottom: BottomAppIconImageSource = appInfo.IconImage; break;
		}
	}

	private void LaunchSelectedApp()
	{
		var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
		if (config is null) return;

		var position = GetSelectedPosition();
		if (position is null) return;

		var path = config.GetByPosition(position.Value).AppPath;
		if (!string.IsNullOrEmpty(path))
			_launchAppService.LaunchApp(path);
	}

	private AppPosition? GetSelectedPosition()
	{
		if (IsSelectedLeft)   return AppPosition.Left;
		if (IsSelectedTop)    return AppPosition.Top;
		if (IsSelectedRight)  return AppPosition.Right;
		if (IsSelectedBottom) return AppPosition.Bottom;
		return null;
	}
}
