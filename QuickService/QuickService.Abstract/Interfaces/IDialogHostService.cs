namespace QuickService.Abstract.Interfaces;

/// <summary>
/// 다이얼로그 서비스 추상화 인터페이스
/// </summary>
public interface IDialogHostService
{
	/// <summary>
	/// 다이얼로그 호스트를 호출한다.
	/// </summary>
	/// <param name="hostName">호출할 다이얼로그 호스트 이름</param>
	/// <param name="content">다이얼로그에 표시할 콘텐츠</param>
	/// <returns>Ok = true / Cancel = false</returns>
	bool ShowDialog(string hostName, object content);
}
