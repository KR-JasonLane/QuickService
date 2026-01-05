using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;

public class LaunchAppService : ILaunchAppService
{
	/// <summary>
	/// 선택된 경로의 어플리케이션을 실행
	/// </summary>
	/// <param name="path">실행할 어플리케이션의 경로</param>
	public void LaunchApp(string path)
	{
		if (string.IsNullOrEmpty(path))
			throw new ArgumentException("어플리케이션 경로는 비어있을 수 없습니다.", nameof(path));

		if (!File.Exists(path))
			throw new FileNotFoundException("어플리케이션을 찾을 수 없습니다.", path);

		Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
	}
}
