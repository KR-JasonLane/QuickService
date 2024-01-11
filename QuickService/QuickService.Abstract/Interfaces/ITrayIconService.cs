namespace QuickService.Abstract.Interfaces;

public interface ITrayIconService
{
	/// <summary>
	/// 작업표시줄에 트레이아이콘을 생성
	/// </summary>
	/// <param name="isVisible"> 트레이아이콘 표시 여부 </param>
	void VisibleTrayIconOnTaskBar(bool isVisible);
}
