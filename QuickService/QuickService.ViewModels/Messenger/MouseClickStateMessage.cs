using QuickService.ViewModels.Messenger.Parameters;

namespace QuickService.ViewModels.Messenger;

public class MouseClickStateMessage : ValueChangedMessage<MouseClickParameter>
{
	public MouseClickStateMessage(MouseClickParameter param) : base(param) { }
}
