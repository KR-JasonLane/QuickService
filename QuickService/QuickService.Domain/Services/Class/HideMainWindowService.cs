namespace QuickService.Domain.Services;

public class HideMainWindowService : IHideMainWindowService
{
	public void HideMainWindow()
	{
		Window mainWindow = Application.Current.MainWindow;

		if (mainWindow != null) mainWindow.Hide();
	}
}
