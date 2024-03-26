using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace CredECard.Common.BusinessService
{
    /// <summary>
    /// this class contain method for resizing images.
    /// </summary>
    public class ImageResize
	{
		/// <author>Prahsant soni</author>
		/// <created>27/03/2005</created>
		/// <summary>Resize image
		/// </summary>
		/// <param name="postedContent">
		/// </param>
		/// <param name="imageHeight">
		/// </param>
		/// <param name="imageWidth">
		/// </param>
		/// <returns>Bitmap for resized image
		/// </returns>
		public static Bitmap Resize(byte[] postedContent,int imageHeight,int imageWidth)
		{						
            MemoryStream ms = new MemoryStream();
            try
            {
                ms.Write(postedContent, 0, postedContent.Length);
                System.Drawing.Image imgSource = System.Drawing.Image.FromStream(ms);

                System.Drawing.Bitmap imgDestination = new Bitmap(imageWidth, imageHeight, PixelFormat.Format24bppRgb);
                imgDestination.SetResolution(imgSource.HorizontalResolution,
                    imgSource.VerticalResolution);

                Graphics grfxPhoto = Graphics.FromImage(imgDestination);
                try
                {
                    grfxPhoto.SmoothingMode = SmoothingMode.HighQuality;
                    grfxPhoto.CompositingQuality = CompositingQuality.HighQuality;
                    grfxPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    grfxPhoto.DrawImage(imgSource,
                        new Rectangle(0, 0, imageWidth, imageHeight), new Rectangle(0, 0, imgSource.Width, imgSource.Height), GraphicsUnit.Pixel);
                }
                catch
                {
                    if (grfxPhoto != null) grfxPhoto.Dispose();
                }
                return imgDestination; 
            }
            finally
            {
                ms.Close();
            }
		}	

        /// <summary>
        /// Mthod resized Image to the given height and width 
        /// </summary>
        /// <param name="postedImage">Image</param>
        /// <param name="imageHeight">int</param>
        /// <param name="imageWidth">int</param>
        /// <returns>Bitmap</returns>
		public static Bitmap Resize(Image postedImage,int imageHeight,int imageWidth)
		{
			MemoryStream stream = new MemoryStream();
			if (postedImage != null)
				postedImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);	
			return Resize(stream.ToArray(),imageHeight,imageWidth);
		}
	}
}
