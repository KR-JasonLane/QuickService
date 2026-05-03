namespace QuickService.Extensions;

/// <summary>
/// Stream 기반 BitmapImage 생성 공통 헬퍼
/// </summary>
internal static class ImageConversionHelper
{
	/// <summary>
	/// Stream에 이미지를 쓰는 Action을 받아 BitmapImage로 변환
	/// </summary>
	/// <param name="writeToStream"> Stream에 이미지 데이터를 쓰는 Action </param>
	/// <returns> 변환된 BitmapImage </returns>
	public static BitmapImage CreateBitmapImage(Action<Stream> writeToStream)
	{
		using var stream = new MemoryStream();
		writeToStream(stream);
		stream.Position = 0;

		var image = new BitmapImage();
		image.BeginInit();
		image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
		image.CacheOption = BitmapCacheOption.OnLoad;
		image.StreamSource = stream;
		image.EndInit();
		image.Freeze();
		return image;
	}
}
