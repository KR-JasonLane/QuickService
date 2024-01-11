using QuickService.Abstract.Interfaces;

namespace QuickService.Domain.Services;

public class TrayIconService : ITrayIconService
{
	public TrayIconService()
	{
		CreateTrayIconObject();
	}

	#region Properties

	/// <summary>
	/// 윈도우를 Hide시켰을 때 생성 될 트레이 아이콘.
	/// </summary>
	private NotifyIcon? _trayIcon;

	#endregion

	#region Methods

	/// <summary>
	/// 작업표시줄에 트레이아이콘을 생성
	/// </summary>
	/// <param name="isVisible"> 트레이아이콘 표시 여부 </param>
	public void VisibleTrayIconOnTaskBar(bool isVisible)
	{
		_trayIcon!.Visible = isVisible;
	}

	/// <summary>
	/// 트레이 아이콘 객체 생성
	/// </summary>
	private void CreateTrayIconObject()
	{
		Icon	icon		= Properties.Resources.QuickService;
		string	processName	= Process.GetCurrentProcess().ProcessName;
		Window	mainWindow	= Application.Current.MainWindow;

		//프로젝트명에 포함되어있는 '.' 이후 글자 삭제
		int findDotIndex = processName.IndexOf('.');

		if (findDotIndex != -1)
			processName = processName.Substring(0, findDotIndex);

		_trayIcon = new()
		{
			Icon	= icon,
			Text	= processName,
			Visible = false
		};

		_trayIcon.DoubleClick += (sender, args) =>
		{
			mainWindow.Show();
			mainWindow.WindowState = WindowState.Normal;
			_trayIcon.Visible = false;
		};

		_trayIcon.ContextMenuStrip = GetTrayIconMenuStrip();
	}

	/// <summary>
	/// 메뉴 스트립 생성
	/// </summary>
	/// <returns> 생성 된 메뉴스트립 </returns>
	private ContextMenuStrip GetTrayIconMenuStrip()
	{
		Window mainWindow = Application.Current.MainWindow;

		ToolStripMenuItem menuItemOpen = new ToolStripMenuItem("Open");
		menuItemOpen.Click += (sender, args) =>
		{
			_trayIcon!.Visible = false;
			mainWindow.Show();
		};

		ToolStripMenuItem menuItemClose = new ToolStripMenuItem("Close");
		menuItemClose.Click += (sender, args) =>
		{
			_trayIcon!.Visible = false;
			Process.GetCurrentProcess().Kill();
		};

		ContextMenuStrip menuStrip = new ContextMenuStrip();
		menuStrip.Items.Add(menuItemOpen);
		menuStrip.Items.Add(menuItemClose);

		return menuStrip;
	}

	#endregion
}
