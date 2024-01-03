using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class TitleViewModel : ObservableRecipient, IViewModel
{
    public TitleViewModel()
    {
        ////////////////////////////////////////
        // 뷰모델 기본 속성
        ////////////////////////////////////////
        {

        }


        ////////////////////////////////////////
        // 바인딩 속성
        ////////////////////////////////////////
        {

        }


        ////////////////////////////////////////
        // 뷰모델 의존성
        ////////////////////////////////////////
        {

        }
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
