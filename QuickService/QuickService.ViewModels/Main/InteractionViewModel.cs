﻿using QuickService.Abstract.Interfaces;
using QuickService.Models.Configure;
using QuickService.ViewModels.Messenger;

namespace QuickService.ViewModels;

public partial class InteractionViewModel : ObservableRecipient, IViewModel
{
    public InteractionViewModel(IUserSelectPathService userSelectPathService, IConfigurationService configurationService)
    {
        ////////////////////////////////////////
        // 서비스 등록
        ////////////////////////////////////////
        {
            _userSelectPathService = userSelectPathService;
			_configurationService  = configurationService;
		}
    }

    #region Services

    /// <summary>
    /// 사용자 선택 경로 서비스
    /// </summary>
    IUserSelectPathService _userSelectPathService;

    /// <summary>
    /// 사용자 설정 서비스
    /// </summary>
    IConfigurationService _configurationService;

    #endregion

    #region Commands

    /// <summary>
    /// URL파라미터 오픈
    /// </summary>
    /// <param name="url"> 오픈할 URL </param>
    [RelayCommand]
    private void OpenWebSiteLink(string url)
	{
		var sInfo = new ProcessStartInfo(url)
		{
			UseShellExecute = true,
		};
		Process.Start(sInfo);
	}

    /// <summary>
    /// 사용자가 선택한 어플리케이션을 기억
    /// </summary>
    /// <param name="param"> LEFT, TOP, RIGHT, BOTTOM 파라미터 </param>
    [RelayCommand]
    private void RegistrationQuickServiceApplication(string param)
    {
        var configuration       = _configurationService.GetConfiguration<RegisteredApplicationModel>();
        string userSelectedPath = _userSelectPathService.GetUserSelectedFilePath();

        if(userSelectedPath != null)
		{
			switch (param)
			{
				case "LEFT"  : configuration.LeftAppInformation.AppPath   = userSelectedPath; break;
				case "TOP"   : configuration.TopAppInformation.AppPath    = userSelectedPath; break;
				case "RIGHT" : configuration.RightAppInformation.AppPath  = userSelectedPath; break;
				case "BOTTOM": configuration.BottomAppInformation.AppPath = userSelectedPath; break;
			}

            _configurationService.SaveConfiguration(configuration);

            var positions = Enum.GetNames(typeof(AppPosition)).ToList();
            int positionIndex = 0;

            foreach (var position in positions)
            {
                if(position.Equals(param))
                {
					WeakReferenceMessenger.Default.Send(new AppInformationChangedMessage((AppPosition)positionIndex));
                    break;
				}
				positionIndex++;
			}            
		}        
    }

    #endregion

    #region Methods

    #endregion
}
