using QuickService.Abstract.Interfaces;
using QuickService.ViewModels.Main;

namespace QuickService.ViewModels;

public partial class TitleViewModel : ObservableRecipient, IViewModel
{
	private readonly ITrayIconService _trayIconService;
	private readonly IHideMainWindowService _hideMainWindowService;
	private readonly IDialogHostService _dialogHostService;

	public TitleViewModel(
		ITrayIconService trayIconService,
		IHideMainWindowService hideMainWindowService,
		IDialogHostService dialogHostService)
	{
		_trayIconService       = trayIconService;
		_hideMainWindowService = hideMainWindowService;
		_dialogHostService     = dialogHostService;
	}

	/// <summary>
	/// 프로세스 종료 커맨드
	/// </summary>
	[RelayCommand]
	private void KillProcess() => Application.Current.Shutdown();

	/// <summary>
	/// 메인윈도우를 숨기고 트레이 아이콘 설치
	/// </summary>
	[RelayCommand]
	private void HideWindowAndVisibleTrayIcon()
	{
		_hideMainWindowService.HideMainWindow();
		_trayIconService.VisibleTrayIconOnTaskBar(true);
	}

	/// <summary>
	/// 옵션 다이얼로그 표시
	/// </summary>
	[RelayCommand]
	private void ShowOptionDialog()
	{
		var content = new ConfigDialogViewModel();
		_dialogHostService.ShowDialog("ShellWindowHost", content);
	}
}
