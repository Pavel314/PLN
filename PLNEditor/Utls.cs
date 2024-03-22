using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace PLNEditor
{
    public enum MouseState { Leave, Over, Down }

    public static class Utils{

        public static bool StrEqualsIgnoreCase(string a,string b)
        {
           return StringComparer.OrdinalIgnoreCase.Equals(a, b);
        }

        public static bool EqualsPath(string a, string b)
        {
            a = Path.GetFullPath(a);
            b = Path.GetFullPath(b);

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Xbox:
                case PlatformID.WinCE:
                    return StringComparer.OrdinalIgnoreCase.Equals(a, b);
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return StringComparer.Ordinal.Equals(a, b);
                default: return StringComparer.OrdinalIgnoreCase.Equals(a, b);
            }

        }

        }

   public static class StyleUtls
    {
    }
    //TODO Если нет шрифта ?
    public static class FontProvider
    {
        public static Font Consolas = new Font("Consolas", 9, FontStyle.Regular);
        public static Font Arial = new Font("Arial", 9, FontStyle.Regular);
    }

    public static class ImageResize
    {
        public static Bitmap Resize(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }

}
