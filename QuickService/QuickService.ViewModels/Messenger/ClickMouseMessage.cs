namespace QuickService.ViewModels.Messenger;

public class ClickMouseMessage : ValueChangedMessage<bool>
{
	public ClickMouseMessage(bool isDown) : base(isDown) { }
}
