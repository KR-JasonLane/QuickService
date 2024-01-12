namespace QuickService.ViewModels.Messenger;

/// <summary>
/// 사용자가 선택한 어플리케이션의 정보가 바뀌었음을 알려주는 메시지
/// </summary>
public class AppInformationChangedMessage : ValueChangedMessage<AppPosition>
{
	public AppInformationChangedMessage(AppPosition position) : base(position) { }
}
