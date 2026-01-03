using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;

public partial class InteractionViewModel : ObservableRecipient, IViewModel
{
    public InteractionViewModel(IUserSelectPathService userSelectPathService, IConfigurationService configurationService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _userSelectPathService = userSelectPathService;
			_configurationService  = configurationService;
		}
    }

    #region Services

    /// <summary>
    /// 사용자 선택 경로 서비스
    /// </summary>
    IUserSelectPathService _userSelectPathService;

    /// <summary>
    /// 사용자 설정 서비스
    /// </summary>
    IConfigurationService _configurationService;

    #endregion

    #region Commands

    /// <summary>
    /// URL파라미터 오픈
    /// </summary>
    /// <param name="url"> 오픈할 URL </param>
    [RelayCommand]
    private void OpenWebSiteLink(string url)
	{
		var sInfo = new ProcessStartInfo(url)
		{
			UseShellExecute = true,
		};
		Process.Start(sInfo);
	}

    /// <summary>
    /// 사용자가 선택한 어플리케이션을 기억
    /// </summary>
    /// <param name="param"> Left, Top, Right, Bottom 파라미터 </param>
    [RelayCommand]
    private void RegistrationQuickServiceApplication(string param)
    {
        string userSelectedPath = _userSelectPathService.GetUserSelectedFilePath();
        if (string.IsNullOrEmpty(userSelectedPath)) return;

        var position = Enum.Parse<AppPosition>(param, ignoreCase: true);
        var configuration = _configurationService.GetConfiguration<RegisteredApplicationModel>();

        configuration.GetByPosition(position).AppPath = userSelectedPath;
        _configurationService.SaveConfiguration(configuration);

        WeakReferenceMessenger.Default.Send(new AppInformationChangedMessage(position));
    }

    #endregion

    #region Methods

    #endregion
}
