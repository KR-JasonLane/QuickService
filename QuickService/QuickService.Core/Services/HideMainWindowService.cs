using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;

public class HideMainWindowService : IHideMainWindowService
{
	/// <summary>
	/// 메인윈도우를 트레이아이콘에 숨김
	/// </summary>
	public void HideMainWindow()
	{
		Window mainWindow = Application.Current.MainWindow;

		if (mainWindow != null) mainWindow.Hide();
	}
}
