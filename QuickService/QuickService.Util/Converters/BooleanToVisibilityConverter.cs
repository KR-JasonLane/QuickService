namespace QuickService.Util.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
	/// <summary>
	/// true일 경우 변환 결과를 반전
	/// </summary>
	public bool IsReversed { get; set; }

	/// <summary>
	/// bool 값을 Visibility 형태로 컨버팅
	/// </summary>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var boolValue = value is bool b && b;
		if (IsReversed) boolValue = !boolValue;
		return boolValue ? Visibility.Visible : Visibility.Collapsed;
	}

	/// <summary>
	/// 컨버팅한 값을 원래 값으로 되돌림
	/// </summary>
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		var isVisible = value is Visibility v && v == Visibility.Visible;
		return IsReversed ? !isVisible : isVisible;
	}
}
