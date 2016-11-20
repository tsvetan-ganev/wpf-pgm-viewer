namespace PGMViewer.Models
{
    public class PGMImage
    {
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
    }
}
