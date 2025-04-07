namespace QuickService.Core.Converters;

public class BooleanToVisibilityReverseConverter : IValueConverter
{
	/// <summary>
	/// bool 형태의 값을 반전시켜 Visibility 형태로 변환
	/// </summary>
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return boolValue ? Visibility.Collapsed : Visibility.Visible;
		}
		return Visibility.Collapsed;
	}

	/// <summary>
	/// 변환값을 원래형태로 되돌림
	/// </summary>
	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is Visibility visibility) && (visibility == Visibility.Visible);
	}
}
