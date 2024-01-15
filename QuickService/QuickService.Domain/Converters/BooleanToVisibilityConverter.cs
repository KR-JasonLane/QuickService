namespace QuickService.Domain.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
	/// <summary>
	/// bool 값을 Visibility 형태로 컨버팅
	/// </summary>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return boolValue ? Visibility.Visible : Visibility.Collapsed;
		}
		return Visibility.Visible;
	}

	/// <summary>
	/// 컨버팅한 값을 원래 값으로 되돌림
	/// </summary>
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is Visibility visibility) && (visibility == Visibility.Visible);
	}
}
