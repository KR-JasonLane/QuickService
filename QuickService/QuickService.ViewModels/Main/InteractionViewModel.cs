using QuickService.Abstract.Interfaces;
using QuickService.Domain.Services;

namespace QuickService.ViewModels;

public partial class InteractionViewModel : ObservableRecipient, IViewModel
{
    public InteractionViewModel(IUserSelectPathService userSelectPathService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _userSelectPathService = userSelectPathService;
        }
    }

    #region Properties

    /// <summary>
    /// 사용자 선택 경로 서비스
    /// </summary>
    IUserSelectPathService _userSelectPathService;

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
    /// <param name="param"> LEFT, TOP, RIGHT, BOTTOM 파라미터 </param>
    [RelayCommand]
    private void RegistrationQuickServiceApplication(string param)
    {
        // TODO : 파라미터에 따른 위치의 어플리케이션을 등록.
        switch(param)
        {
            case "LEFT":
                //_userSelectPathService.GetUserSelectedFilePath();
                break;

            case "TOP":
                //_userSelectPathService.GetUserSelectedFilePath();
                break;

            case "RIGHT":
                //_userSelectPathService.GetUserSelectedFilePath();
                break;

            case "BOTTOM":
                //_userSelectPathService.GetUserSelectedFilePath();
                break;
        }
        
    }

    #endregion

    #region Methods

    #endregion
}
