using QuickService.Abstract.Interfaces;
using QuickService.ViewModels.Messenger;
using QuickService.ViewModels.Messenger.Parameters;

namespace QuickService.Core.Services;

public class GlobalMouseHookService : IGlobalMouseHookService
{
	private const int WH_MOUSE_LL    = 14;
	private const int WM_MOUSEMOVE   = 0x0200;
	private const int WM_LBUTTONUP   = 0x0202;
	private const int WM_LBUTTONDOWN = 0x0201;

	private readonly LowLevelMouseProc _mouseProc;
	private static IntPtr _hookHandle = IntPtr.Zero;
	private bool _isModifierKeyDown;

	public GlobalMouseHookService()
	{
		_mouseProc = MouseHookProc;

		WeakReferenceMessenger.Default.Register<ModifierKeyStateMessage>(this, (r, m) =>
			_isModifierKeyDown = m.Value);
	}

	#region dll Imports

	[DllImport("user32.dll")]
	static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc callback, IntPtr hInstance, uint threadId);

	[DllImport("user32.dll")]
	static extern bool UnhookWindowsHookEx(IntPtr hInstance);

	[DllImport("user32.dll")]
	static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	static extern IntPtr LoadLibrary(string lpFileName);

	#endregion

	private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

	/// <summary>
	/// 전역후킹 설정
	/// </summary>
	public void SetHook()
	{
		IntPtr hInstance = LoadLibrary("User32");
		_hookHandle = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, hInstance, 0);
	}

	/// <summary>
	/// 전역후킹 해제
	/// </summary>
	public void UnHook()
	{
		UnhookWindowsHookEx(_hookHandle);
	}

	/// <summary>
	/// 후킹한 마우스 이벤트 핸들링
	/// </summary>
	public IntPtr MouseHookProc(int code, IntPtr wParam, IntPtr lParam)
	{
		if (code >= 0 && _isModifierKeyDown)
			DispatchMouseEvent((int)wParam);

		return CallNextHookEx(_hookHandle, code, (int)wParam, lParam);
	}

	private static void DispatchMouseEvent(int eventType)
	{
		switch (eventType)
		{
			case WM_LBUTTONDOWN: SendClickMessage(isDown: true);  break;
			case WM_LBUTTONUP:   SendClickMessage(isDown: false); break;
			case WM_MOUSEMOVE:   SendMoveMessage();               break;
		}
	}

	private static void SendClickMessage(bool isDown)
	{
		var param = new MouseClickParameter
		{
			IsDown = isDown,
			X = isDown ? Cursor.Position.X : 0,
			Y = isDown ? Cursor.Position.Y : 0
		};
		WeakReferenceMessenger.Default.Send(new MouseClickStateMessage(param));
	}

	private static void SendMoveMessage()
	{
		var param = new MouseCoordinateParameter
		{
			X = Cursor.Position.X,
			Y = Cursor.Position.Y
		};
		WeakReferenceMessenger.Default.Send(new MouseMoveMessage(param));
	}
}
