using QuickService.Abstract.Interfaces;
using QuickService.ViewModels.Messenger;

namespace QuickService.Core.Services;

public class GlobalKeyboardHookService : IGlobalKeyboardHookService
{
	private const int WH_KEYBOARD_LL = 13;
	private const int WM_KEYUP      = 0x0101;
	private const int WM_KEYDOWN    = 0x0104;
	private const int VK_ALT        = 164;

	private readonly LowLevelKeyboardProc _keyboardProc;
	private static IntPtr _hookHandle = IntPtr.Zero;

	public GlobalKeyboardHookService()
	{
		_keyboardProc = KeyboardHookProc;
	}

	#region dll Imports

	[DllImport("user32.dll")]
	static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

	[DllImport("user32.dll")]
	static extern bool UnhookWindowsHookEx(IntPtr hInstance);

	[DllImport("user32.dll")]
	static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	static extern IntPtr LoadLibrary(string lpFileName);

	#endregion

	private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

	/// <summary>
	/// 전역후킹 설정
	/// </summary>
	public void SetHook()
	{
		IntPtr hInstance = LoadLibrary("User32");
		_hookHandle = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, hInstance, 0);
	}

	/// <summary>
	/// 전역후킹 해제
	/// </summary>
	public void UnHook()
	{
		UnhookWindowsHookEx(_hookHandle);
	}

	/// <summary>
	/// 후킹한 키보드 이벤트 핸들링
	/// </summary>
	public IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
	{
		if (code >= 0)
			HandleKeyEvent((int)wParam, Marshal.ReadInt32(lParam));

		return CallNextHookEx(_hookHandle, code, (int)wParam, lParam);
	}

	private static void HandleKeyEvent(int eventType, int vkCode)
	{
		if (vkCode != VK_ALT) return;

		if (eventType == WM_KEYDOWN)
			WeakReferenceMessenger.Default.Send(new ModifierKeyStateMessage(true));
		else if (eventType == WM_KEYUP)
			WeakReferenceMessenger.Default.Send(new ModifierKeyStateMessage(false));
	}
}
