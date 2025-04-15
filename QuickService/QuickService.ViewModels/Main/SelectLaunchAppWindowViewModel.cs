using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;

public partial class SelectLaunchAppWindowViewModel : ObservableRecipient, IViewModel
{
	public SelectLaunchAppWindowViewModel(IConfigurationService configurationService, ILaunchAppService launchAppService)
	{
		////////////////////////////////////////
		// 서비스 등록
		////////////////////////////////////////
		{
			_configurationService = configurationService;
			_launchAppService     = launchAppService;
		}


		////////////////////////////////////////
		// 메신저 등록
		////////////////////////////////////////
		{
			WeakReferenceMessenger.Default.Register<MouseClickStateMessage>(this, (r, m) =>
			{
				IsWindowOpen   = m.Value.IsDown;
				IsNoneSelect   = true          ;

				if (m.Value.IsDown)
				{
					PositionLeft = m.Value.X - (WindowLength / 2);
					PositionTop  = m.Value.Y - (WindowLength / 2);

					_currentWindowCenter = new Point(m.Value.X, m.Value.Y);

					CalculateMousePoint(_currentWindowCenter);
				}
				else
				{
					LaunchSelectedApp();
				}
			});

			WeakReferenceMessenger.Default.Register<ModifierKeyStateMessage>(this, (r, m) =>
			{
				if (m.Value is false)
					IsWindowOpen = false;
			});

			WeakReferenceMessenger.Default.Register<MouseMoveMessage>(this, (r, m) =>
			{
				if (IsWindowOpen) 
					CalculateMousePoint(new Point(m.Value.X, m.Value.Y));
			});

			WeakReferenceMessenger.Default.Register<AppInformationChangedMessage>(this, (r, m) =>
			{
				LoadUserSelectedAppIconImages(m.Value);
			});
		}


		////////////////////////////////////////
		// 바인딩 속성
		////////////////////////////////////////
		{
			WindowLength        = 300; //윈도우 크기
			InvalidAreaDiameter = 52 ; //무효영역 크기

			LoadUserSelectedAppIconImages(AppPosition.LEFT  );
			LoadUserSelectedAppIconImages(AppPosition.TOP   );
			LoadUserSelectedAppIconImages(AppPosition.RIGHT );
			LoadUserSelectedAppIconImages(AppPosition.BOTTOM);
		}
	}

	#region Properties

	/// <summary>
	/// 사용자 설정 서비스
	/// </summary>
	private IConfigurationService _configurationService;

	/// <summary>
	/// 어플리케이션 실행 서비스
	/// </summary>
	private ILaunchAppService _launchAppService;

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
	/// 윈도우가 Show됨을 알림
	/// </summary>
	[ObservableProperty]
	private bool _isWindowOpen;


	/// <summary>
	/// 좌측 앱 아이콘 이미지
	/// </summary>
	[ObservableProperty]
	private ImageSource _leftAppIconImageSource;

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

	/// <summary>
	/// 윈도우의 Left좌표
	/// </summary>
	[ObservableProperty]
	private double _positionLeft;

	/// <summary>
	/// 윈도우의 Top좌표
	/// </summary>
	[ObservableProperty]
	private double _positionTop;

	/// <summary>
	/// 윈도우의 가로,세로 길이
	/// </summary>
	[ObservableProperty]
	private double _windowLength;

	/// <summary>
	/// 무효영역의 지름
	/// </summary>
	[ObservableProperty]
	private double _invalidAreaDiameter;

	/// <summary>
	/// 현재 윈도우의 가운데 좌표 저장
	/// </summary>
	private Point _currentWindowCenter;

	#endregion

	#region Commands

	#endregion

	#region Methods

	/// <summary>
	/// 마우스 좌표를 계산
	/// </summary>
	/// <param name="mousePosition"> 현재 마우스의 위치 </param>
	private void CalculateMousePoint(Point mousePosition)
	{
		if(CheckMouseInInvalidArea(mousePosition))
		{
			TurnOffSelectedFlags();
			IsNoneSelect = true;
			return;
		}

		IsNoneSelect = false;

		double deltaX = mousePosition       .X - _currentWindowCenter.X;
		double deltaY = _currentWindowCenter.Y - mousePosition       .Y;
		double angle = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

		PointerAngle = -angle + 90;

		CalculatePointedPosition(angle);
	}

	/// <summary>
	/// 마우스가 무효영역에 있는지 확인
	/// </summary>
	/// <param name="mouse"> 현재 마우스 위치 </param>
	/// <returns> 마우스가 무효영역에 위치해 있는지 여부</returns>
	private bool CheckMouseInInvalidArea(Point mousePosition)
	{
		double ellipseRadiusX = InvalidAreaDiameter / 2;
		double ellipseRadiusY = InvalidAreaDiameter / 2;

		double distance = Math.Sqrt(Math.Pow(mousePosition.X - _currentWindowCenter.X, 2) / Math.Pow(ellipseRadiusX, 2) +
									Math.Pow(mousePosition.Y - _currentWindowCenter.Y, 2) / Math.Pow(ellipseRadiusY, 2));

		return distance <= 1.0;
	}

	/// <summary>
	/// 마우스가 가리키고있는 영역을 계산
	/// </summary>
	/// <param name="angle"> 마우스의 각도</param>
	private void CalculatePointedPosition(double angle)
	{
		TurnOffSelectedFlags();

		if(angle < 0) angle += 360;

		IsSelectedTop    = angle >= 45  && angle < 135;
		IsSelectedLeft   = angle >= 135 && angle < 225;
		IsSelectedBottom = angle >= 225 && angle < 315;
		IsSelectedRight  = angle >= 315 || angle < 45;

		if (IsSelectedLeft || IsSelectedTop || IsSelectedRight || IsSelectedBottom)
		{
			IsNoneSelect = false;
		}
	}

	/// <summary>
	/// 모든 선택플래그를 끔
	/// </summary>
	private void TurnOffSelectedFlags()
	{
		IsSelectedLeft   = false;
		IsSelectedTop    = false;
		IsSelectedRight  = false;
		IsSelectedBottom = false;
	}

	/// <summary>
	/// 사용자가 선택한 어플리케이션의 아이콘을 이미지로 저장
	/// </summary>
	/// <param name="position"> 설정 하고자 하는 영역 </param>
	private void LoadUserSelectedAppIconImages(AppPosition position)
	{
		var configuration = _configurationService.GetConfiguration<RegisteredApplicationModel>();

		if(configuration != null)
		{
			switch(position)
			{
				case AppPosition.LEFT:
					if(configuration.LeftAppInformation.IsValidPath())
					{
						LeftAppIconImageSource = configuration.LeftAppInformation.GetIconImage()!;
					}
				break;

				case AppPosition.TOP:
					if(configuration.TopAppInformation.IsValidPath())
					{
						TopAppIconImageSource = configuration.TopAppInformation.GetIconImage()!;
					}
				break;

				case AppPosition.RIGHT:
					if(configuration.RightAppInformation.IsValidPath())
					{
						RightAppIconImageSource = configuration.RightAppInformation.GetIconImage()!;
					}
				break;

				case AppPosition.BOTTOM:
					if(configuration.BottomAppInformation.IsValidPath())
					{
						BottomAppIconImageSource = configuration.BottomAppInformation.GetIconImage()!;
					}
				break;
			}
		}
	}

	/// <summary>
	/// 선택된 영역의 프로그램을 실행
	/// </summary>
	private void LaunchSelectedApp()
	{
		var configuration = _configurationService.GetConfiguration<RegisteredApplicationModel>();

        if (configuration != null)
        {
			if      (IsSelectedLeft  ) _launchAppService.LaunchApp(configuration.LeftAppInformation.AppPath!  );
			else if (IsSelectedTop   ) _launchAppService.LaunchApp(configuration.TopAppInformation.AppPath!   );
			else if (IsSelectedRight ) _launchAppService.LaunchApp(configuration.RightAppInformation.AppPath! );
			else if (IsSelectedBottom) _launchAppService.LaunchApp(configuration.BottomAppInformation.AppPath!);
		}        
	}

	#endregion
}
