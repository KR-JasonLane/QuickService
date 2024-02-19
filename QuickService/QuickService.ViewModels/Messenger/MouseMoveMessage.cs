using QuickService.ViewModels.Messenger.Parameters;

namespace QuickService.ViewModels.Messenger;

public class MouseMoveMessage : ValueChangedMessage<MouseCoordinateParameter>
{
	public MouseMoveMessage(MouseCoordinateParameter param) : base(param) { }
}
