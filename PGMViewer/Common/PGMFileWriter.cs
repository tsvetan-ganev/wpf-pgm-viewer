using System.IO;
using PGMViewer.Models;

namespace PGMViewer.Common
{
    public static class PGMFileWriter
    {
        public static void Save(FileStream fs, PGMImage pgmImg)
        {
            using (var writer = new StreamWriter(fs))
            {
                // first line - the PGM's magic number
                writer.WriteLine(pgmImg.MagicNumber);

                // second line - the name of the PGM written as a comment
                if (!string.IsNullOrWhiteSpace(pgmImg.Name))
                {
                    writer.WriteLine("# " + pgmImg.Name);
                }

                // third line - width and height
                writer.WriteLine(pgmImg.Width + " " + pgmImg.Height);

                // fourth line - maximal grey value
                writer.WriteLine(pgmImg.MaxGrayValue);

                // fifth line and ownwards - image's pixel data
                string pixelLine = "";
                string currentPixel = "";
                const int MAX_LINE_LENGTH = 70; // no line should be longer than 70 symbols

                for (int i = 0; i < pgmImg.PixelData.Length; i++)
                {
                    currentPixel = pgmImg.PixelData[i] + " ";
                    if (pixelLine.Length + currentPixel.Length >= MAX_LINE_LENGTH)
                    {
                        writer.WriteLine(pixelLine);
                        pixelLine = currentPixel;
                    }
                    else
                    {
                        pixelLine += currentPixel;
                    }
                }
            }
        }
    }
}
