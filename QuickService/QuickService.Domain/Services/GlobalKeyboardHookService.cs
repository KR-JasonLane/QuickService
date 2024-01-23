using QuickService.Abstract.Interfaces;

namespace QuickService.Domain.Services;

public class GlobalKeyboardHookService : IGlobalKeyboardHookService
{
	public GlobalKeyboardHookService(IGlobalMouseHookService mouseHookService)
	{
		////////////////////////////////////////
		// 키보드 이벤트
		////////////////////////////////////////
		{
			WH_KEYBOARD_LL  = 13;
			WM_KEYUP		= 0x0101;
			WM_KEYDOWN		= 0x0104;
			VK_ALT			= 164;
		}


		////////////////////////////////////////
		// 대리자 설정
		////////////////////////////////////////
		{
			_keyboardProc = KeyboardHookProc;
		}


		////////////////////////////////////////
		// 대리자 설정
		////////////////////////////////////////
		{
			_mouseHookService = mouseHookService;
		}
	}

	#region dll Imports

	/// <summary>
	/// 전역키보드 후킹 설정
	/// </summary>
	[DllImport("user32.dll")]
	static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

	/// <summary>
	/// 전역후킹 해제
	/// </summary>
	[DllImport("user32.dll")]
	static extern bool UnhookWindowsHookEx(IntPtr hInstance);

	/// <summary>
	/// 키보드 이벤트 넘겨주기
	/// </summary>
	[DllImport("user32.dll")]
	static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

	/// <summary>
	/// 라이브러리 로드
	/// </summary>
	[DllImport("kernel32.dll")]
	static extern IntPtr LoadLibrary(string lpFileName);

	#endregion

	#region delegate

	/// <summary>
	/// 마우스 후킹 프로시저 대리자 등록
	/// </summary>
	private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

	#endregion

	#region Properties

	/// <summary>
	/// 키보드 이벤트 핸들
	/// </summary>
	private readonly int WH_KEYBOARD_LL;

	/// <summary>
	/// 키 업 이벤트 
	/// </summary>
	private readonly int WM_KEYUP;

	/// <summary>
	/// 키 다운 이벤트
	/// </summary>
	private readonly int WM_KEYDOWN;

	/// <summary>
	/// Alt키 코드
	/// </summary>
	private readonly int VK_ALT;

	/// <summary>
	/// 키보드 프로시저 대리자 객체
	/// </summary>
	private readonly LowLevelKeyboardProc _keyboardProc;

	/// <summary>
	/// 현재 후킹중인 핸들
	/// </summary>
	private static IntPtr _handleHookMouse = IntPtr.Zero;

	/// <summary>
	/// 마우스 후킹 서비스
	/// </summary>
	private readonly IGlobalMouseHookService _mouseHookService;

	#endregion

	#region Methods

	/// <summary>
	/// 전역후킹 설정
	/// </summary>
	public void SetHook()
	{
		IntPtr hInstance = LoadLibrary("User32");
		_handleHookMouse = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, hInstance, 0);
	}

	/// <summary>
	/// 전역후킹 해제
	/// </summary>
	public void UnHook()
	{
		UnhookWindowsHookEx(_handleHookMouse);
	}

	/// <summary>
	/// 후킹한 키보드 이벤트 핸들링
	/// </summary>
	public IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam)
	{
		if (code >= 0)
		{
			int vkCode = Marshal.ReadInt32(lParam);

			if (vkCode == VK_ALT)
			{
				if (wParam == WM_KEYUP)
				{
					_mouseHookService.SetModifierKeyDownState(false);
				}
				else if (wParam == WM_KEYDOWN)
				{
					_mouseHookService.SetModifierKeyDownState(true);
				}
			}
		}

		return CallNextHookEx(_handleHookMouse, code, (int)wParam, lParam);
	}

	#endregion
}
