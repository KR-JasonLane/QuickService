namespace QuickService.Abstract.Interfaces;

public interface IJsonFileService
{
	/// <summary>
	/// 객체를 JSON 파일로 저장
	/// </summary>
	/// <typeparam name="T">저장할 객체의 타입</typeparam>
	/// <param name="obj">저장할 객체</param>
	/// <param name="path">저장할 파일 경로</param>
	void Save<T>(T obj, string path);

	/// <summary>
	/// JSON 파일을 객체로 로드
	/// </summary>
	/// <typeparam name="T">로드할 객체의 타입 (기본생성자 필요)</typeparam>
	/// <param name="path">로드할 파일 경로</param>
	/// <returns>역직렬화된 객체</returns>
	T Load<T>(string path) where T : new();
}
