namespace QuickService.ViewModels.Messenger.Parameters;

public class MouseClickParameter
{
	/// <summary>
	/// 마우스 클릭상태
	/// </summary>
	public bool IsDown { get; set; }

	/// <summary>
	/// 마우스의 X좌표
	/// </summary>
	public double X { get; set; }

	/// <summary>
	/// 마우스의 Y좌표
	/// </summary>
	public double Y { get; set; }
}
