using QuickService.Abstract.Interfaces;

namespace QuickService.Domain.Services;
public class UserSelectPathService : IUserSelectPathService
{
    /// <summary>
    /// 유저가 선택한 파일의 경로를 반환
    /// </summary>
    /// <returns> 파일의 경로 </returns>
    public string GetUserSelectedFilePath()
    {
        OpenFileDialog fileDlg = new OpenFileDialog();

        fileDlg.DefaultExt  = ".exe";
        fileDlg.Filter      = "EXE Files (*.exe)|*.exe|All Files (*.*)|*.*";
        fileDlg.Multiselect = false;

        if (fileDlg.ShowDialog() == DialogResult.OK) 
            return fileDlg.FileName;
        else 
            return string.Empty;
    }
}
