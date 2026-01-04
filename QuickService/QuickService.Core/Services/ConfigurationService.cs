using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;

public class ConfigurationService : IConfigurationService
{
	private readonly string _configFilePath;
	private readonly IJsonFileService _jsonFileService;

	public ConfigurationService(IJsonFileService jsonFileService)
	{
		_jsonFileService = jsonFileService ?? throw new ArgumentNullException(nameof(jsonFileService));
		_configFilePath  = Path.Combine(Directory.GetCurrentDirectory(), "Config", "Configuration.json");
	}

	/// <summary>
	/// 환경설정 객체를 저장
	/// </summary>
	public void SaveConfiguration<T>(T configurationObj)
	{
		ArgumentNullException.ThrowIfNull(configurationObj);
		_jsonFileService.Save(configurationObj, _configFilePath);
	}

	/// <summary>
	/// 환경설정을 로드
	/// </summary>
	public T GetConfiguration<T>() where T : new()
	{
		return _jsonFileService.Load<T>(_configFilePath);
	}
}
