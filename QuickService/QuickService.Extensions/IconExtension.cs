using System.Drawing.Imaging;

namespace QuickService.Extensions;

/// <summary>
/// Icon 확장메서드
/// </summary>
public static class IconExtension
{
	/// <summary>
	/// Icon에서 이미지 추출
	/// </summary>
	/// <param name="icon"> 이미지를 추출 할 아이콘 객체 </param>
	/// <returns> 추출한 이미지 </returns>
	public static ImageSource ToImageSource(this Icon icon)
	{
		ArgumentNullException.ThrowIfNull(icon);
		return ImageConversionHelper.CreateBitmapImage(stream =>
		{
			using var bitmap = icon.ToBitmap();
			bitmap.Save(stream, ImageFormat.Png);
		});
	}
}
