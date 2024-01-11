namespace QuickService.Abstract.Interfaces;
public interface IUserSelectPathService
{
	/// <summary>
	/// 유저가 선택한 파일의 경로를 반환
	/// </summary>
	/// <returns> 파일의 경로 </returns>
	string GetUserSelectedFilePath();
}
