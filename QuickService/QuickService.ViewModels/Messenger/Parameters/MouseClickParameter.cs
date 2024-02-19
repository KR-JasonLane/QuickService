namespace QuickService.ViewModels.Messenger.Parameters;

public class MouseClickParameter : MouseCoordinateParameter
{
	/// <summary>
	/// 마우스 클릭상태
	/// </summary>
	public bool IsDown { get; set; }
}
