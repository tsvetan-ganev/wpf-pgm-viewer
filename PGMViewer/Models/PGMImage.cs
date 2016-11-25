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
    }
}
