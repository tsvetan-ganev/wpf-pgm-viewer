namespace PGMViewer.Models
{
    public class BorderSettings
    {
        private byte _greyLevel;
        private uint _width;

        public byte GreyLevel
        {
            get { return _greyLevel; }
            set
            {
                if (value < 0)
                    _greyLevel = 0;
                else if (value > 255)
                    _greyLevel = 255;
                else
                    _greyLevel = value;
            }
        }

        public uint Width { get; set; }
    }
}
