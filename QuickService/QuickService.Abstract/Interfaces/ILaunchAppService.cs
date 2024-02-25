namespace QuickService.Abstract.Interfaces;

public interface ILaunchAppService
{
	/// <summary>
	/// 어플리케이션을 실행
	/// </summary>
	/// <param name="path"> 실행할 경로 </param>
	void LaunchApp(string path);
}
