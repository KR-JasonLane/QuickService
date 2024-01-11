namespace QuickService.Views.Behaviors;

public sealed class TitleLeftButtonDownDehavior : Behavior<FrameworkElement>
{
	#region Override methods

	/// <summary>
	/// 활성화
	/// </summary>
	protected override void OnAttached()
	{
		AssociatedObject.MouseLeftButtonDown += AssociatedObjectLeftButtonDown;
	}

	/// <summary>
	/// 비활성화
	/// </summary>
	protected override void OnDetaching()
	{
		AssociatedObject.MouseLeftButtonDown -= AssociatedObjectLeftButtonDown;
	}

	#endregion

	#region Methods

	/// <summary>
	/// 메인 윈도우 이동
	/// </summary>
	/// <param name="sender"> 이벤트 발생객체 </param>
	/// <param name="mouseEventArgs"> 마우스 이벤트 파라미터 </param>
	private void AssociatedObjectLeftButtonDown(object sender, MouseEventArgs mouseEventArgs) => Application.Current.MainWindow.DragMove();

	#endregion
}
