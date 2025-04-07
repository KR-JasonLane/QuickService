using QuickService.Abstract.Interfaces;
using QuickService.ViewModels.Messenger;
using QuickService.ViewModels.Messenger.Parameters;

namespace QuickService.Core.Services;

public class GlobalMouseHookService : IGlobalMouseHookService
{
	public GlobalMouseHookService()
	{
		////////////////////////////////////////
		// 메신저 등록
		////////////////////////////////////////
		{
			WeakReferenceMessenger.Default.Register<ModifierKeyStateMessage>(this, (r, m) =>
			{
				_isModifierKeyDown = m.Value;
			});
		}


		////////////////////////////////////////
		// 마우스 이벤트 메시지
		////////////////////////////////////////
		{
			WH_MOUSE_LL    = 14;
			WM_MOUSEMOVE   = 0x0200;
			WM_LBUTTONUP   = 0x0202;
			WM_LBUTTONDOWN = 0x0201;
		}


		////////////////////////////////////////
		// 대리자 설정
		////////////////////////////////////////
		{
			_mouseProc = MouseHookProc;
		}
	}

	#region dll Imports

	/// <summary>
	/// 전역마우스 후킹 설정
	/// </summary>
	[DllImport("user32.dll")]
	static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc callback, IntPtr hInstance, uint threadId);

	/// <summary>
	/// 전역후킹 해제
	/// </summary>
	[DllImport("user32.dll")]
	static extern bool UnhookWindowsHookEx(IntPtr hInstance);

	/// <summary>
	/// 마우스 이벤트 넘겨주기
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
	private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

	#endregion

	#region Properties

	/// <summary>
	/// 마우스 이벤트 핸들
	/// </summary>
	private readonly int WH_MOUSE_LL;

	/// <summary>
	/// 마우스 무브 이벤트
	/// </summary>
	private readonly int WM_MOUSEMOVE;

	/// <summary>
	/// 마우스 업 이벤트
	/// </summary>
	private readonly int WM_LBUTTONUP;

	/// <summary>
	/// 마우스 다운 이벤트
	/// </summary>
	private readonly int WM_LBUTTONDOWN;

	/// <summary>
	/// 마우스 프로시저 대리자 객체
	/// </summary>
	private readonly LowLevelMouseProc _mouseProc;

	/// <summary>
	/// 현재 후킹중인 핸들
	/// </summary>
	private static IntPtr _handleHookMouse = IntPtr.Zero;

	/// <summary>
	/// 조합키 눌림 여부
	/// </summary>
	private bool _isModifierKeyDown;

	#endregion

	#region Methods

	/// <summary>
	/// 전역후킹 설정
	/// </summary>
	public void SetHook()
	{
		IntPtr hInstance = LoadLibrary("User32");
		_handleHookMouse = SetWindowsHookEx(WH_MOUSE_LL, _mouseProc, hInstance, 0);
	}

	/// <summary>
	/// 전역후킹 해제
	/// </summary>
	public void UnHook()
	{
		UnhookWindowsHookEx(_handleHookMouse);
	}

	/// <summary>
	/// 후킹한 마우스 이벤트 핸들링
	/// </summary>
	public IntPtr MouseHookProc(int code, IntPtr wParam, IntPtr lParam)
	{
		if (code >= 0 && _isModifierKeyDown is true)
		{
			if(wParam == WM_LBUTTONDOWN)
			{
				var param = new MouseClickParameter()
				{
					IsDown = true,
					X = Cursor.Position.X,
					Y = Cursor.Position.Y
				};

				WeakReferenceMessenger.Default.Send(new MouseClickStateMessage(param));
			}
			else if (wParam == WM_LBUTTONUP)
			{
				var param = new MouseClickParameter()
				{
					IsDown = false,
				};

				WeakReferenceMessenger.Default.Send(new MouseClickStateMessage(param));
			}
			else if (wParam == WM_MOUSEMOVE)
			{
				var param = new MouseCoordinateParameter()
				{
					X = Cursor.Position.X,
					Y = Cursor.Position.Y
				};

				WeakReferenceMessenger.Default.Send(new MouseMoveMessage(param));
			}
		}

		return CallNextHookEx(_handleHookMouse, code, (int)wParam, lParam);
	}

	#endregion
}
