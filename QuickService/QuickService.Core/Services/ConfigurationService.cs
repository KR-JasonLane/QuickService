using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;
public class ConfigurationService : IConfigurationService
{
	public ConfigurationService(IJsonFileService? jsonFileService)
	{
		////////////////////////////////////////
		// 고유속성
		////////////////////////////////////////
		{
			_configFilePath	= Path.Combine(Directory.GetCurrentDirectory(), "Config", "Configuration.json");
		}

		////////////////////////////////////////
		// 서비스 등록
		////////////////////////////////////////
		{
			_jsonFileService = jsonFileService;
		}
	}

	#region Properties

	/// <summary>
	/// 설정파일의 경로를 기억
	/// </summary>
	private readonly string? _configFilePath;

	/// <summary>
	/// Json파일 핸들링 서비스
	/// </summary>
	private readonly IJsonFileService? _jsonFileService;

	#endregion

	#region Methods

	/// <summary>
	/// 환경설정 객체를 저장
	/// </summary>
	/// <typeparam name="T"> 저장될 객체의 타입 </typeparam>
	/// <param name="configurationObj"> 환경설정 속성을 지니고 있는 객체 </param>
	public void SaveConfiguration<T>(T configurationObj)
	{
		if (configurationObj != null)
		{
			_jsonFileService!.SaveJsonProperties(configurationObj, _configFilePath!);
		}
	}

	/// <summary>
	/// 환경설정을 로드
	/// </summary>
	/// <typeparam name="T"> 로드할 객체의 타입(기본생성자) </typeparam>
	/// <returns> 로드한 객체 </returns>
	public T GetConfiguration<T>() where T : new()
	{
		return _jsonFileService!.GetJsonProperties<T>(_configFilePath!);
	}

	#endregion


}
