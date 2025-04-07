using QuickService.Abstract.Interfaces;

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
		return true;
	}
}
