using QuickService.Abstract.Interfaces;
using QuickService.ViewModels.Main;
using QuickService.Views.UserControls;

namespace QuickService.Core.Services;

/// <summary>
/// 다이얼로그 서비스 구현
/// </summary>
public class DialogHostService : IDialogHostService
{
	/// <summary>
	/// 다이얼로그 호스트를 호출한다.
	/// </summary>
	/// <param name="dialogHostName"> 호출할 다이얼로그 호스트 이름 </param>
	/// <param name="message"> 메시지(기본값 = 빈 문자열) </param>
	/// <returns> Ok = true / Cancel = false </returns>
	public bool ShowConfigDialog(string dialogHostName, string message = "")
	{
		//다이얼로그 뷰 생성
		object? configDialogView = new ConfigDialogView() { DataContext = new ConfigDialogViewModel() };

		//다이얼로그 띄우기
		object? result = DialogHost.Show(configDialogView, dialogHostName);

		//결과 반환
		return result is true;
	}
}
