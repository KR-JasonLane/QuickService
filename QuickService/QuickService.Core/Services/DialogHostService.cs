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
	/// <param name="hostName">호출할 다이얼로그 호스트 이름</param>
	/// <param name="content">다이얼로그에 표시할 콘텐츠</param>
	/// <returns>Ok = true / Cancel = false</returns>
	public bool ShowDialog(string hostName, object content)
	{
		// DialogHost.Show는 비동기 메서드로, 다이얼로그가 닫힐 때까지 대기하지 않음
		// 호출부에서 반환값을 사용하지 않으므로 fire-and-forget 처리
		_ = DialogHost.Show(content, hostName);
		return true;
	}
}
