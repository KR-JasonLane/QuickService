using QuickService.Domain.Extensions;

namespace QuickService.Models.Configure;

public class AppInformationModel
{
	/// <summary>
	/// 아이콘 이미지
	/// </summary>
	[JsonIgnore]
	public ImageSource? IconImage { get; set; }

	/// <summary>
	/// 어플리케이션 이름
	/// </summary>
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// 어플리케이션 경로
	/// </summary>
	private string? _appPath;
	public string? AppPath
	{ 
		get => _appPath!;
		set => SetInformationPropertyFromPath(ref _appPath!, value!);
	}

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
		else
		{
			//TODO : Empty이미지 사용하기
		}		
	}
}
