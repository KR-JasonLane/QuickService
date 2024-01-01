﻿using QuickService.Abstract.Interfaces;

namespace QuickService.ViewModels;

public partial class MainViewModel : ObservableRecipient, IViewModel
{
	public MainViewModel(IViewModel titleViewModel, 
						 IViewModel interactionViewModel, 
						 IViewModel selectedFileViewModel)
	{
		////////////////////////////////////////
		// 뷰모델 기본 속성
		////////////////////////////////////////
		{

		}


		////////////////////////////////////////
		// 바인딩 속성
		////////////////////////////////////////
		{

		}


        ////////////////////////////////////////
        // 뷰모델 의존성
        ////////////////////////////////////////
        {
			TitleContent		= titleViewModel;
			InteractionContent	= interactionViewModel;
			SelectedFileContent = selectedFileViewModel;
        }
	}

	#region Properties

	/// <summary>
	/// 타이틀 뷰모델
	/// </summary>
	[ObservableProperty]
	private IViewModel _titleContent;

	/// <summary>
	/// 상호작용 뷰모델
	/// </summary>
	[ObservableProperty]
	private IViewModel _interactionContent;

	/// <summary>
	/// 선택된 파일표시 뷰모델
	/// </summary>
	[ObservableProperty]
	private IViewModel _selectedFileContent;

	#endregion

	#region Methods

	#endregion
}
