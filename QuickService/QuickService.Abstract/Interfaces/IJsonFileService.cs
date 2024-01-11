namespace QuickService.Abstract.Interfaces;

public interface IJsonFileService
{
	/// <summary>
	/// 폴더경로와 파일경로가 유효한지 확인하고 유효하지 않으면 기본값으로 생성
	/// </summary>
	/// <returns> 결과 반환 </returns>
	bool SaveJsonProperties<T>(T jsonObject, string jsonPath);

	/// <summary>
	/// Json파일 파싱
	/// </summary>
	/// <returns> Json 파싱데이터 </returns>
	T GetJsonProperties<T>(string jsonPath) where T : new();
}
