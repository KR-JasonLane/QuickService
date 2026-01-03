using Newtonsoft.Json;
using QuickService.Extensions;

namespace QuickService.Models.Configure;

public class AppInformationModel
{
	/// <summary>
	/// 어플리케이션 경로
	/// </summary>
	public string? AppPath { get; set; }

	/// <summary>
	/// 아이콘 이미지 (직렬화 제외)
	/// </summary>
	[JsonIgnore]
	public ImageSource? IconImage { get; private set; }

	/// <summary>
	/// 어플리케이션 표시 이름 (직렬화 제외)
	/// </summary>
	[JsonIgnore]
	public string? DisplayName { get; private set; }

	/// <summary>
	/// 경로가 유효한지 검사
	/// </summary>
	/// <returns>유효성 여부</returns>
	public bool HasValidPath() => !string.IsNullOrEmpty(AppPath) && File.Exists(AppPath);

	/// <summary>
	/// 경로로부터 아이콘과 이름 정보를 로드
	/// </summary>
	public void LoadInfoFromPath()
	{
		if (!HasValidPath()) return;

		IconImage   = Icon.ExtractAssociatedIcon(AppPath!)?.ToImageSource();
		DisplayName = Path.GetFileNameWithoutExtension(AppPath);
	}

	/// <summary>
	/// 경로가 유효한지 검사
	/// </summary>
	[Obsolete("HasValidPath()를 사용하세요.")]
	public bool IsValidPath() => HasValidPath();

	/// <summary>
	/// 아이콘 이미지를 반환
	/// </summary>
	[Obsolete("IconImage 프로퍼티를 직접 사용하세요.")]
	public ImageSource? GetIconImage() => IconImage;

	/// <summary>
	/// 어플리케이션 이름을 반환
	/// </summary>
	[Obsolete("DisplayName 프로퍼티를 직접 사용하세요.")]
	public string? GetAppName() => DisplayName;
}
