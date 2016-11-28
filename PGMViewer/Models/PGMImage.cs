using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PGMViewer.Models
{
    public class PGMImage
    {
        private const byte DPI = 96;
        private const double ACTUAL_MAX_GREY_LEVEL = 255.0d;

        private uint _maxGrayValue;

        public string Name { get; set; }

        public string MagicNumber { get; set; }

        public uint Width { get; set; }

        public uint Height { get; set; }

        public uint MaxGrayValue
        {
            get { return _maxGrayValue; }
            set
            {
                if (_maxGrayValue > 65535)
                    _maxGrayValue = 65535;
                else
                    _maxGrayValue = value;

            }
        }

        public byte[] PixelData { get; set; }

        public double GreyLevelScale
        {
            get
            {
                return this.MaxGrayValue / ACTUAL_MAX_GREY_LEVEL;
            }
        }

        public bool HasValidMetadata
        {
            get
            {
                return this.Width > 0 
                    && this.Height > 0 
                    && this.MaxGrayValue > 0 
                    && !string.IsNullOrWhiteSpace(this.MagicNumber);
            }
        }

        public BitmapSource ToBitmapSource()
        {
            return BitmapSource.Create(
                pixelWidth: (int) this.Width,
                pixelHeight: (int) this.Height,
                dpiX: DPI,
                dpiY: DPI,
                pixelFormat: PixelFormats.Gray8,
                palette: null,
                pixels: this.PixelData,
                stride: (int) this.Width
            );
        }

        public void Save(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                if (fileName.EndsWith(".pgm"))
                {
                    SaveAsPGM(fileStream);
                }
                else
                {
                    SaveAsImage(fileStream, fileName);
                }
            }
        }

        private void SaveAsPGM(FileStream fs)
        {
            using (var writer = new StreamWriter(fs))
            {
                // first line - the PGM's magic number
                writer.WriteLine(this.MagicNumber);

                // second line - the name of the PGM written as a comment
                if (!string.IsNullOrWhiteSpace(this.Name))
                {
                    writer.WriteLine("# " + this.Name);
                }

                // third line - width and height
                writer.WriteLine(this.Width + " " + this.Height);

                // fourth line - maximal grey value
                writer.WriteLine(this.MaxGrayValue);

                // fifth line and ownwards - image's pixel data
                string pixelLine = "";
                string currentPixel = "";
                const int MAX_LINE_LENGTH = 70; // no line should be longer than 70 symbols

                for (int i = 0; i < this.PixelData.Length; i++)
                {
                    currentPixel = this.PixelData[i] + " ";
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

        private void SaveAsImage(FileStream fs, string fileName)
        {
            BitmapEncoder encoder = null;

            if (fileName.EndsWith(".jpg"))
            {
                encoder = new JpegBitmapEncoder();
            }
            else if (fileName.EndsWith(".png"))
            {
                encoder = new PngBitmapEncoder();
            }
            else
            {
                encoder = new BmpBitmapEncoder();
            }

            encoder.Frames.Add(BitmapFrame.Create(this.ToBitmapSource()));
            encoder.Save(fs);
        }
    }
}
