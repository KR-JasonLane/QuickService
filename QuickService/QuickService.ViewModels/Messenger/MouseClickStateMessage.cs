namespace QuickService.ViewModels.Messenger;

public class MouseClickStateMessage : ValueChangedMessage<bool>
{
	public MouseClickStateMessage(bool isDown) : base(isDown) { }
}
