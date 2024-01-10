namespace QuickService.Models.Configure;

public class ConfigureModel
{
	/// <summary>
	/// 사용자가 등록한 어플리케이션 경로 목록
	/// </summary>
	public RegisteredApplicationModel RegisteredApplication { get; set; } = new();
}
