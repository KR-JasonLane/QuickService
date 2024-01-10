namespace QuickService.Models.Configure;

public class RegisteredApplicationModel
{
	/// <summary>
	/// 좌측에 저장된 어플리케이션의 정보
	/// </summary>
	public AppInformationModel LeftAppInformation { get; set; } = new();

	/// <summary>
	/// 상단에 저장된 어플리케이션의 정보
	/// </summary>
	public AppInformationModel TopAppInformation { get; set; } = new();

	/// <summary>
	/// 우측에 저장된 어플리케이션의 정보
	/// </summary>
	public AppInformationModel RightAppInformation { get; set; } = new();

	/// <summary>
	/// 하단에 저장된 어플리케이션의 정보
	/// </summary>
	public AppInformationModel BottomAppInformation { get; set; } = new();
}
