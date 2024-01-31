namespace QuickService.Abstract.Interfaces;

public interface IGlobalMouseHookService
{
	/// <summary>
	/// 마우스 후킹 설정
	/// </summary>
	void SetHook();

	/// <summary>
	/// 마우스 후킹 설정 해제
	/// </summary>
	void UnHook();
}
