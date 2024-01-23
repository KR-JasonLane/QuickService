using QuickService.Extensions;

namespace QuickService.Models.Configure;

public class AppInformationModel
{

	#region Properties

	/// <summary>
	/// 아이콘 이미지
	/// </summary>
	private ImageSource? IconImage { get; set; }

	/// <summary>
	/// 어플리케이션 이름
	/// </summary>
	private string? Name { get; set; }

	/// <summary>
	/// 어플리케이션 경로
	/// </summary>
	private string? _appPath;
	public string? AppPath
	{ 
		get => _appPath!;
		set => SetInformationPropertyFromPath(ref _appPath!, value!);
	}

	#endregion

	#region Methods

	/// <summary>
	/// 경로에서 속성들을 추출
	/// </summary>
	/// <param name="sender"> 저장할 객체 </param>
	/// <param name="input"> 입력된 객체 </param>
	private void SetInformationPropertyFromPath(ref string sender, string input)
	{
		if(File.Exists(input)) 
		{
			IconImage	= Icon.ExtractAssociatedIcon(input)!.ToImageSource();
			Name		= Path.GetFileName(input);
			sender		= input;
		}
	}

	/// <summary>
	/// 생성된 아이콘이미지를 반환
	/// </summary>
	/// <returns> 아이콘 이미지 </returns>
	public ImageSource? GetIconImage() => IconImage;

	/// <summary>
	/// 어플리케이션의 이름을 반환
	/// </summary>
	/// <returns> 어플리케이션의 이름 </returns>
	public string? GetAppName() => Name;

	/// <summary>
	/// 경로가 유효한지 검사
	/// </summary>
	/// <returns> 유효성 여부 </returns>
	public bool IsValidPath() => Path.Exists(AppPath);

	#endregion
}
