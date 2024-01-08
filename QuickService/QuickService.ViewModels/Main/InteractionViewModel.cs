using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class InteractionViewModel : ObservableRecipient, IViewModel
{
    public InteractionViewModel()
    {

    }

    #region Properties

    #endregion

    #region Methods

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
    private void MemorizedQuickServiceApplication(string param)
    {
        // TODO : 파라미터에 따른 위치의 어플리케이션을 등록.
    }

    #endregion
}
