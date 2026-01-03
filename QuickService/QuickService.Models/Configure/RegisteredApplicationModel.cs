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

	/// <summary>
	/// 위치에 해당하는 어플리케이션 정보를 반환
	/// </summary>
	/// <param name="position">어플리케이션 위치</param>
	/// <returns>해당 위치의 어플리케이션 정보</returns>
	/// <exception cref="ArgumentOutOfRangeException">유효하지 않은 위치</exception>
	public AppInformationModel GetByPosition(AppPosition position) => position switch
	{
		AppPosition.Left   => LeftAppInformation,
		AppPosition.Top    => TopAppInformation,
		AppPosition.Right  => RightAppInformation,
		AppPosition.Bottom => BottomAppInformation,
		_ => throw new ArgumentOutOfRangeException(nameof(position), position, "유효하지 않은 어플리케이션 위치입니다.")
	};
}
