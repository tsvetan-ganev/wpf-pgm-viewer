using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PGMViewer.Models
{
    public class PGMImage
    {
        private const int DPI = 96;

        private uint _maxGrayValue;

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
