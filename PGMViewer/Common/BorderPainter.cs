using System;
using PGMViewer.Models;

namespace PGMViewer.Common
{
    public class BorderPainter
    {
        public static byte[] AddBorder(PGMImage img, uint borderWidth, byte color)
        {
            byte[] pixels = new byte[img.Width * img.Height];
            Array.Copy(img.PixelData, pixels, img.Width * img.Height);

            // Adds top and bottom borders
            for (uint y = 0; y < borderWidth; y++)
            {
                uint topOffset = y * img.Width;
                uint bottomOffset = (img.Height - y - 1) * img.Width;
                for (uint x = 0; x < img.Width; x++)
                {
                    pixels[topOffset + x] = color;
                    pixels[bottomOffset + x] = color;
                }
            }

            // Adds left and right borders
            for (uint x = 0; x < borderWidth; x++)
            {
                for (uint y = 0; y < img.Height; y++)
                {
                    uint yOffset = y * img.Width;
                    pixels[yOffset + x] = color;
                    pixels[yOffset + img.Width - x - 1] = color;
                }
            }

            return pixels;
        }
    }
}
