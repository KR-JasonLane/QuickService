namespace QuickService.Abstract.Interfaces;

public interface IGlobalKeyboardHookService
{
	/// <summary>
	/// 키보드 후킹 설정
	/// </summary>
	void SetHook();

	/// <summary>
	/// 키보드 후킹 설정 해제
	/// </summary>
	void UnHook();
}
