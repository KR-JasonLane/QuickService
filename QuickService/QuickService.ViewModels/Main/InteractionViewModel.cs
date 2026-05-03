using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;

public partial class InteractionViewModel : ObservableRecipient, IViewModel
{
	private readonly IUserSelectPathService _userSelectPathService;
	private readonly IConfigurationService _configurationService;

	public InteractionViewModel(
		IUserSelectPathService userSelectPathService,
		IConfigurationService configurationService)
	{
		_userSelectPathService = userSelectPathService;
		_configurationService  = configurationService;
	}

	/// <summary>
	/// URL파라미터 오픈
	/// </summary>
	[RelayCommand]
	private void OpenWebSiteLink(string url)
	{
		Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
	}

	/// <summary>
	/// 사용자가 선택한 어플리케이션을 기억
	/// </summary>
	[RelayCommand]
	private void RegistrationQuickServiceApplication(string param)
	{
		var path = _userSelectPathService.GetUserSelectedFilePath();
		if (string.IsNullOrEmpty(path)) return;

		var position = Enum.Parse<AppPosition>(param, ignoreCase: true);
		SaveAppRegistration(position, path);
		NotifyAppChanged(position);
	}

	private void SaveAppRegistration(AppPosition position, string path)
	{
		var config = _configurationService.GetConfiguration<RegisteredApplicationModel>();
		config.GetByPosition(position).AppPath = path;
		_configurationService.SaveConfiguration(config);
	}

	private static void NotifyAppChanged(AppPosition position)
	{
		WeakReferenceMessenger.Default.Send(new AppInformationChangedMessage(position));
	}
}
