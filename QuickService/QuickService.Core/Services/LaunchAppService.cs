using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;

public class LaunchAppService : ILaunchAppService
{

	/// <summary>
	/// 선택된 경로의 어플리케이션을 실행
	/// </summary>
	/// <param name="path"> 실행 할 어플리케이션의 경로 </param>
	public void LaunchApp(string path)
	{
		if (string.IsNullOrEmpty(path)) return;

		try
		{
			if (File.Exists(path))
			{
				Process.Start(path);
			}
			else
			{
				System.Windows.MessageBox.Show("Invalid file path");
			}
		}
		catch (Exception ex)
		{
			System.Windows.MessageBox.Show($"Fatal error : {ex.Message}");
		}
	}
}
