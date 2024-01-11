using QuickService.Abstract.Interfaces;

namespace QuickService.Domain.Services;

public class JsonFileService : IJsonFileService
{

	/// <summary>
	/// 폴더경로와 파일경로가 유효한지 확인하고 유효하지 않으면 기본값으로 생성
	/// </summary>
	/// <returns> 결과 반환 </returns>
	public bool SaveJsonProperties<T>(T jsonObject, string jsonPath)
	{
		string directoryPath = Path.GetDirectoryName(jsonPath)!;

		////////////////////////////////////////
		// 폴더경로, 파일경로 입력검사
		////////////////////////////////////////
		{
			if (string.IsNullOrEmpty(directoryPath) || string.IsNullOrEmpty(jsonPath))
			{
				throw new Exception($"Error : Invalid path.");
			}
		}


		////////////////////////////////////////
		// 폴더 생성 시도
		////////////////////////////////////////
		{
			try					{ Directory.CreateDirectory(directoryPath); }
			catch (Exception e) { throw new Exception(e.Message);			}
		}


		////////////////////////////////////////
		// 파일 생성 시도
		////////////////////////////////////////
		{
			string convertedJsonText = JsonConvert.SerializeObject(jsonObject, Formatting.None);

			try					{ File.WriteAllText(jsonPath, convertedJsonText);  }
			catch (Exception e) { throw new Exception(e.Message);						}
		}


		////////////////////////////////////////
		// 결과반환
		////////////////////////////////////////
		{
			return File.Exists(jsonPath);
		}
	}

	/// <summary>
	/// Json파일 파싱
	/// </summary>
	/// <returns> Json 파싱데이터 </returns>
	public T GetJsonProperties<T>(string jsonPath) where T : new()
	{
		if (File.Exists(jsonPath) == false)
		{
			T result = new();
			SaveJsonProperties(result, jsonPath);

			return result;
		}
		else
		{
			T? result = JsonConvert.DeserializeObject<T>(File.ReadAllText(jsonPath));

			if (result == null)
			{
				result = new();
				SaveJsonProperties(result, jsonPath);
			}

			return result;
		}
	}
}
