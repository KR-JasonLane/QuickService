using QuickService.Abstract.Interfaces;
using QuickService.Domain.Services;

namespace QuickService.ViewModels;

public partial class TitleViewModel : ObservableRecipient, IViewModel
{
    public TitleViewModel(ITrayIconService trayIconService, IHideMainWindowService hideMainWindowService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _trayIconService        = trayIconService;
            _hideMainWindowService  = hideMainWindowService;
        }
    }

    #region Properties

    /// <summary>
    /// 트레이 아이콘 서비스
    /// </summary>
    private readonly ITrayIconService _trayIconService;

    /// <summary>
    /// 메인윈도우 Hide 서비스
    /// </summary>
    private readonly IHideMainWindowService _hideMainWindowService;

    #endregion

    #region RelayCommand

    /// <summary>
    /// 프로세스 종료 커맨드
    /// </summary>
    [RelayCommand]
    private void KillProcess() => Process.GetCurrentProcess().Kill();

    [RelayCommand]
    private void HideWindowAndVisibleTrayIcon()
    {
        _hideMainWindowService.HideMainWindow();
        _trayIconService.VisibleTrayIconOnTaskBar(true);
    }

	#endregion

	#region Methods

	#endregion
}
