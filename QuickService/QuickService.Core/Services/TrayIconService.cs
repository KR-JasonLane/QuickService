using QuickService.Abstract.Interfaces;

namespace QuickService.Core.Services;

public class TrayIconService : ITrayIconService
{
	private readonly NotifyIcon _trayIcon;

	public TrayIconService()
	{
		_trayIcon = CreateTrayIcon();
	}

	/// <summary>
	/// 작업표시줄에 트레이아이콘 표시 여부 설정
	/// </summary>
	public void VisibleTrayIconOnTaskBar(bool isVisible)
	{
		_trayIcon.Visible = isVisible;
	}

	private NotifyIcon CreateTrayIcon()
	{
		var icon = new NotifyIcon
		{
			Icon    = Properties.Resources.QuickService,
			Text    = GetApplicationName(),
			Visible = false,
		};
		icon.DoubleClick += (_, _) => RestoreMainWindow();
		icon.ContextMenuStrip = CreateContextMenu();
		return icon;
	}

	private static string GetApplicationName()
	{
		var name = Process.GetCurrentProcess().ProcessName;
		var dotIndex = name.IndexOf('.');
		return dotIndex >= 0 ? name[..dotIndex] : name;
	}

	private ContextMenuStrip CreateContextMenu()
	{
		var menu = new ContextMenuStrip();
		menu.Items.Add(CreateMenuItem("Open", (_, _) => RestoreMainWindow()));
		menu.Items.Add(CreateMenuItem("Close", (_, _) => Application.Current.Shutdown()));
		return menu;
	}

	private static ToolStripMenuItem CreateMenuItem(string text, EventHandler handler)
	{
		var item = new ToolStripMenuItem(text);
		item.Click += handler;
		return item;
	}

	private void RestoreMainWindow()
	{
		_trayIcon.Visible = false;
		var window = Application.Current.MainWindow;
		window.Show();
		window.WindowState = WindowState.Normal;
	}
}
