using System;
using System.IO;
using System.Linq;
using PGMViewer.Models;

namespace PGMViewer.Common
{
    public class PGMParser
    {
        public static PGMImage Parse(string filePath)
        {
            return _ParsePMGFile(filePath);
        }

        private static PGMImage _ParsePMGFile(string filePath)
        {
            using (TextReader reader = File.OpenText(filePath))
            {
                string currentLine = null;
                var pgm = new PGMImage();

                // read metadata
                while (!pgm.HasValidMetadata && (currentLine = reader.ReadLine()) != null)
                {
                    // skip the empty lines and those containing comments
                    if (string.IsNullOrEmpty(currentLine) || currentLine.StartsWith("#"))
                    {
                        continue;
                    }

                    // it's usually the first line
                    // but it may start with a comment
                    if (pgm.MagicNumber == null)
                    {
                        if (currentLine.Trim() != "P2")
                        {
                            throw new InvalidPGMFormatException("Currently only the P2 (ASCII PGM) format is supported.");
                        }

                        pgm.MagicNumber = currentLine;
                        continue;
                    }

                    var splitted = currentLine.Split(
                        new string[] { " ", Environment.NewLine, "\t" },
                        StringSplitOptions.RemoveEmptyEntries
                    );

                    try
                    {
                        // setting width and height
                        if (pgm.Width == 0 || pgm.Height == 0)
                        {
                            pgm.Width = uint.Parse(splitted[0]);
                            pgm.Height = uint.Parse(splitted[1]);
                            continue;
                        }

                        // setting max gray value
                        if (pgm.MaxGrayValue == 0)
                        {
                            pgm.MaxGrayValue = uint.Parse(splitted[0]);
                        }
                    }
                    catch (FormatException ex)
                    {
                        throw new InvalidPGMFormatException(
                            "PGM's width, height or maximal grey value are not in the required format or are missing.");
                    }
                }

                if (!pgm.HasValidMetadata)
                {
                    throw new InvalidPGMFormatException(
                        "PGM metadata could not be read or is not valid. Please check if your .pgm file complies to the required format."
                    );
                }

                // start reading the pixel data
                byte[] pixelData = new byte[pgm.Width * pgm.Height];
                int totalPixelsReadSoFar = 0;

                while ((currentLine = reader.ReadLine()) != null)
                {
                    // skip the empty lines and those containing comments
                    if (string.IsNullOrEmpty(currentLine) || currentLine.StartsWith("#"))
                    {
                        continue;
                    }

                    try
                    {
                        byte[] pixelValues = currentLine.Split(
                            new string[] { " ", Environment.NewLine, "\t" },
                            StringSplitOptions.RemoveEmptyEntries
                        ).Select(p => byte.Parse(p)).ToArray();

                        for (int i = 0; i < pixelValues.Length; i++)
                        {
                            // prevents byte overflow
                            double tempResult = pgm.GreyLevelScale * pixelValues[i];
                            byte pixel = tempResult > 255 ? (byte) 255 : (byte) tempResult;

                            pixelData[totalPixelsReadSoFar + i] = pixel;
                        }

                        totalPixelsReadSoFar += pixelValues.Length;
                    }
                    catch (FormatException ex)
                    {
                        throw new InvalidPGMFormatException("The PGM pixel data contains values that cannot be converted to bytes.");
                    }
                }

                pgm.PixelData = pixelData;
                return pgm;
            }
        }
    }
}
