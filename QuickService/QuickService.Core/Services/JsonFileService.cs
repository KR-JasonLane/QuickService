using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;

public class JsonFileService : IJsonFileService
{
	/// <summary>
	/// 객체를 JSON 파일로 저장
	/// </summary>
	public void Save<T>(T obj, string path)
	{
		ValidatePath(path);
		EnsureDirectoryExists(path);
		WriteJson(obj, path);
	}

	/// <summary>
	/// JSON 파일을 객체로 로드
	/// </summary>
	public T Load<T>(string path) where T : new()
	{
		if (!File.Exists(path))
			return CreateAndSaveDefault<T>(path);

		return DeserializeOrDefault<T>(path);
	}

	private static void ValidatePath(string path)
	{
		if (string.IsNullOrEmpty(path))
			throw new ArgumentException("파일 경로는 null이거나 빈 문자열일 수 없습니다.", nameof(path));
	}

	private static void EnsureDirectoryExists(string filePath)
	{
		var dir = Path.GetDirectoryName(filePath);
		if (!string.IsNullOrEmpty(dir))
			Directory.CreateDirectory(dir);
	}

	private static void WriteJson<T>(T obj, string path)
	{
		var json = JsonConvert.SerializeObject(obj, Formatting.None);
		File.WriteAllText(path, json);
	}

	private T CreateAndSaveDefault<T>(string path) where T : new()
	{
		var result = new T();
		Save(result, path);
		return result;
	}

	private T DeserializeOrDefault<T>(string path) where T : new()
	{
		var json = File.ReadAllText(path);
		return JsonConvert.DeserializeObject<T>(json) ?? CreateAndSaveDefault<T>(path);
	}
}
