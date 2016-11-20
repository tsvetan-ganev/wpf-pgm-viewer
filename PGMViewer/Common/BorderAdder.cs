using System;
using PGMViewer.Models;

namespace PGMViewer.Common
{
    public class BorderAdder
    {
        public static byte[] AddBorder(PGMImage img, uint borderWidth, byte greyLevel)
        {
            byte[] pixels = new byte[img.Width * img.Height];
            Array.Copy(img.PixelData, pixels, img.Width * img.Height);

            // Adds top border
            for (uint y = 0; y < borderWidth; y++)
            {
                uint yOffset = y * img.Width;
                for (uint x = 0; x < img.Width; x++)
                {
                    pixels[yOffset + x] = greyLevel;
                }
            }

            // TODO: Add the other borders

            return pixels;
        }
    }
}
