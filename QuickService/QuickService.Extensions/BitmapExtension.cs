using System.Drawing.Imaging;

namespace QuickService.Extensions;

/// <summary>
/// Bitmap 확장메서드
/// </summary>
public static class BitmapExtension
{
	/// <summary>
	/// Bitmap객체를 ImageSource로 변환
	/// </summary>
	/// <param name="bitmap"> Bitmap원본 </param>
	/// <returns> 변환한 ImageSource </returns>
	public static ImageSource ToImageSource(this Bitmap bitmap)
	{
		ArgumentNullException.ThrowIfNull(bitmap);
		return ImageConversionHelper.CreateBitmapImage(
			stream => bitmap.Save(stream, ImageFormat.Png));
	}
}
