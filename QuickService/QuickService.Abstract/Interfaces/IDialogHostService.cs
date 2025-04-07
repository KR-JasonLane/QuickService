namespace QuickService.Abstract.Interfaces;

/// <summary>
/// 다이얼로그 서비스 추상화 인터페이스
/// </summary>
public interface IDialogHostService
{
	/// <summary>
	/// 다이얼로그 호스트를 호출한다.
	/// </summary>
	/// <param name="dialogHostName"> 호출할 다이얼로그 호스트 이름 </param>
	/// <param name="message"> 메시지(기본값 = 빈 문자열) </param>
	/// <returns> Ok = true / Cancel = false </returns>
	bool ShowConfigDialog(string dialogHostName, string message = "");
}
