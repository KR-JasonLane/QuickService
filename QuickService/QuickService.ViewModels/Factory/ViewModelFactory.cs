using QuickService.Abstract.Interfaces;
using QuickService.ViewModels;

namespace QuickService.ViewModels.Factory;

/// <summary>
/// 팩토리 매서드 패턴에 따라 View model을 생성.
/// </summary>
public sealed class ViewModelFactory
{
	/// <summary>
	/// 팩토리의 생성자
	/// Service Provider 컨테이너에 대한 의존성을 주입 받아 어셈블리단에 있는 뷰모델 객체를 찾음.
	/// </summary>
	/// <param name="serviceProvider"> 컨테이너 </param>
	public ViewModelFactory(IServiceProvider serviceProvider)
	{
		_serviceProvider	= serviceProvider;
		_viewModelCache		= new Dictionary<string, IViewModel>();
	}

	#region Properties

	/// <summary>
	/// 생성자에서 주입받은 컨테이너
	/// </summary>
	private readonly IServiceProvider _serviceProvider;

	/// <summary>
	/// 생성된 뷰모델 객체를 덤핑
	/// </summary>
	private readonly Dictionary<string, IViewModel> _viewModelCache;

	#endregion

	#region Methods

	/// <summary>
	/// 팩토리에서 뷰모델 객체를 가져온다.
	/// </summary>
	/// <typeparam name="T"> 뷰모델의 클래스 타입 </typeparam>
	/// <param name="keys"> 딕셔너리에서 뷰모델을 찾기위한 키 </param>
	/// <returns> 해당되는 뷰모델의 객체를 반환 </returns>
	internal T GetViewModel<T>(params string[] keys) where T : IViewModel
	{
		string viewModelKey = typeof(T).FullName!;

		if (_viewModelCache.TryGetValue(viewModelKey, out IViewModel? cachedViewModel))
		{
			return (T)cachedViewModel;
		}

		else
		{
			T newViewModel = CreateViewModel<T>(keys);
			return newViewModel;
		}
	}

	/// <summary>
	/// 뷰모델을 생성
	/// </summary>
	/// <typeparam name="T"> 뷰모델의 클래스 타입 </typeparam>
	/// <param name="keys"> 딕셔너리에서 뷰모델을 찾기위한 키 </param>
	/// <returns> 새로 생성된 뷰 모델을 반환 </returns>
	/// <exception cref="InvalidOperationException"> 적절한 생성자를 찾을 수 없을 때 throw 되는 예외 </exception>
	private T CreateViewModel<T>(string[] keys) where T : IViewModel
	{
		string viewModelKey = typeof(T).FullName!;

		List<object> constructorParameters = GetFoundViewModelConstructor(keys);

		Type[] parameterTypes = constructorParameters.Select(param => param.GetType())
			.ToArray();

		ConstructorInfo constructor = typeof(T)
			.GetConstructor(parameterTypes) ?? throw new InvalidOperationException("적절한 생성자를 찾을 수 없습니다.");

		T newViewModel = (T)constructor.Invoke(constructorParameters.ToArray());

		// 새로 생성 된 ViewModel을 덤핑
		_viewModelCache[viewModelKey] = newViewModel;

		return newViewModel;
	}

	/// <summary>
	/// 어셈블리에서 특정 뷰모델의 생성 메서드를 찾는다. (의존성 주입)
	/// </summary>
	/// <param name="keys"> 생성 메서드를 찾기위한 뷰모델 키 </param>
	/// <returns> 생성 메서드를 찾아 만들어진 인스턴스 반환 </returns>
	private List<object> GetFoundViewModelConstructor(string[] keys)
	{
		List<object> newViewModel = new List<object>();

		foreach (string key in keys)
		{
			//해당되는 메서드를 어셈블리에서 찾는다.
			MethodInfo? createMethod = GetType()
				.GetMethod($"Get{key}ViewModel", BindingFlags.Public | BindingFlags.Instance);

			object constructorParameters;

			if (createMethod != null)
			{
				//Invoke(호출할 매서드가 속한 객체, 해당 매서드에 전달 될 매개변수[필요없으면 null])
				constructorParameters = createMethod.Invoke(this, null)!;
			}
			else
			{
				// 특정 ViewModel에 대한 생성 매서드가 없으면 기본 생성자 사용
				Type viewModelType = _serviceProvider.GetRequiredKeyedService<IViewModel>(key).GetType();
				constructorParameters = Activator.CreateInstance(viewModelType)!;
			}
			newViewModel.Add(constructorParameters);
		}

		return newViewModel;
	}

	#endregion

	#region Dependency

	public IViewModel GetMainWindowViewModel() => GetViewModel<MainWindowViewModel>(nameof(MainViewModel)[..^9]);

	public IViewModel GetMainViewModel() => GetViewModel<MainViewModel>();

	#endregion

}
