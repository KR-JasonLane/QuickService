namespace QuickService.Abstract.Interfaces;

public interface IConfigurationService
{
	/// <summary>
	/// 환경설정 객체를 저장
	/// </summary>
	/// <typeparam name="T"> 저장될 객체의 타입 </typeparam>
	/// <param name="configurationObj"> 환경설정 속성을 지니고 있는 객체 </param>
	void SaveConfiguration<T>(T configurationObj);

	/// <summary>
	/// 환경설정을 로드
	/// </summary>
	/// <typeparam name="T"> 로드할 객체의 타입(기본생성자) </typeparam>
	/// <returns> 로드한 객체 </returns>
	T GetConfiguration<T>() where T : new();
}
