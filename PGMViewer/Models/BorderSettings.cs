namespace PGMViewer.Models
{
    public class BorderSettings
    {
        private const byte MAX_GREY_LEVEL = 255;
        private byte _greyLevel;

        public byte GreyLevel
        {
            get { return _greyLevel; }
            set
            {
                if (value < 0)
                    _greyLevel = 0;
                else if (value > MAX_GREY_LEVEL)
                    _greyLevel = MAX_GREY_LEVEL;
                else
                    _greyLevel = value;
            }
        }

        public uint Width { get; set; }
    }
}
