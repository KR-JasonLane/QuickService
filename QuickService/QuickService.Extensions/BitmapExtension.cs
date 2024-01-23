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
	public static ImageSource? ToImageSource(this Bitmap bitmap)
	{
		ImageSource? result = null;

		if (bitmap != null)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
				stream.Position = 0; // Reset the stream position

				BitmapImage bitmapImage = new BitmapImage();
				bitmapImage.BeginInit();
				bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
				bitmapImage.StreamSource = stream;
				bitmapImage.EndInit();

				result = bitmapImage;
			}
		}
		return result;
	}
}
