using System.Drawing;
using QRCoder;

namespace PKHeX.Drawing.Misc;

public class QRCodeBitmapConverter
{
    public static Bitmap GetBitmap(QRCodeData data, int size)
    {
        return GetGraphic(data, size, Color.Black, Color.White, true);
    }
    private static Bitmap GetGraphic(QRCodeData QrCodeData, int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
    {
        var size = (QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
        var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

        var bmp = new Bitmap(size, size);
        using (var gfx = Graphics.FromImage(bmp))
        using (var lightBrush = new SolidBrush(lightColor))
        using (var darkBrush = new SolidBrush(darkColor))
        {
            for (var x = 0; x < size + offset; x += pixelsPerModule)
            {
                for (var y = 0; y < size + offset; y += pixelsPerModule)
                {
                    var module = QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1];

                    if (module)
                    {
                        gfx.FillRectangle(darkBrush, new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                    }
                    else
                    {
                        gfx.FillRectangle(lightBrush, new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                    }
                }
            }

            gfx.Save();
        }

        return bmp;
    }
}
