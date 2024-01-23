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

	/// <summary>
	/// 조합키가 눌려져 있는지에 대한 상태 업데이트
	/// </summary>
	/// <param name="isDown"> 조합키 눌림 여부 </param>
	void SetModifierKeyDownState(bool isDown);
}
