using System.Windows;
using PGMViewer.Models;

namespace PGMViewer.UI
{
    public partial class AddBorderDialog : Window
    {
        public AddBorderDialog(uint imageWidth)
        {
            this.MaxBorderWidth = imageWidth / 2;
            InitializeComponent();
        }

        public uint MaxBorderWidth { get; set; }

        public BorderSettings BorderSettings { get; set; }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            BorderSettings = new BorderSettings()
            {
                GreyLevel = (byte)greyLevelSlider.Value,
                Width = (uint)borderWidthSlider.Value
            };

            DialogResult = true;
        }
    }
}
