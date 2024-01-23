namespace QuickService.ViewModels.Messenger;

public class PressModifierKeyMessage : ValueChangedMessage<bool>
{
	public PressModifierKeyMessage(bool isDown) : base(isDown) { }
}
