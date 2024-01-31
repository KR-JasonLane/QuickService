namespace QuickService.ViewModels.Messenger;

public class ModifierKeyStateMessage : ValueChangedMessage<bool>
{
	public ModifierKeyStateMessage(bool isDown) : base(isDown) { }
}
