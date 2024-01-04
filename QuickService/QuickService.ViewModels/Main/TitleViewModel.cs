using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class TitleViewModel : ObservableRecipient, IViewModel
{
    public TitleViewModel()
    {

    }

    #region Properties

    #endregion

    #region Methods

    /// <summary>
    /// 종료버튼 클릭 커맨드
    /// </summary>
    [RelayCommand]
    private void ClickCloseButton() => Process.GetCurrentProcess().Kill();

	#endregion
}
