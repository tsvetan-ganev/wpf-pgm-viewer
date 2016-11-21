using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PGMViewer.Common;
using PGMViewer.Models;

namespace PGMViewer.UI
{
    public partial class MainWindow : Window
    {
        private const string DEFAULT_TITLE = "PGM Viewer";
        private const int DPI = 96;

        private PGMImage _currentlyOpenedPGM;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Title = DEFAULT_TITLE;
            _UpdateMenuItemsClickableStatus(areEnabled: false);
        }

        private void OnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PGM Files | *.pgm";

            if (openFileDialog.ShowDialog() == true)
            {
                this.Title = openFileDialog.SafeFileName;

                try
                {
                    _currentlyOpenedPGM = PGMParser.Parse(openFileDialog.FileName);
                    var source = _CreateBitmapFromPGM(_currentlyOpenedPGM);
                    renderedImage.Source = source;
                    _UpdateMenuItemsClickableStatus(areEnabled: true);
                }
                catch (InvalidPGMFormatException invalidPgmException)
                {
                    _currentlyOpenedPGM = null;
                    this.Title = DEFAULT_TITLE;
                    MessageBox.Show(invalidPgmException.Message, "Invalid PGM file.");
                }
            }
        }

        private void OnCloseFile_Click(object sender, RoutedEventArgs e)
        {
            _currentlyOpenedPGM = null;
            renderedImage.Source = null;
            this.Title = DEFAULT_TITLE;
            _UpdateMenuItemsClickableStatus(areEnabled: false);
        }

        private void OnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (_currentlyOpenedPGM == null)
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            saveFileDialog.Filter = "Bitmap Picture|*.bmp|JPEG image|*.jpg|PNG image|*.png";


            if (saveFileDialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder = null;

                    if (saveFileDialog.FileName.EndsWith(".jpg"))
                    {
                        encoder = new JpegBitmapEncoder();
                    }
                    else if (saveFileDialog.FileName.EndsWith(".png"))
                    {
                        encoder = new PngBitmapEncoder();
                    }
                    else
                    {
                        encoder = new BmpBitmapEncoder();
                    }

                    encoder.Frames.Add(BitmapFrame.Create(_CreateBitmapFromPGM(_currentlyOpenedPGM)));
                    encoder.Save(fileStream);
                }
            }
        }

        private void onAddBorder_Click(object sender, RoutedEventArgs e)
        {
            var addBorderDialog = new AddBorderDialog();
            if (addBorderDialog.ShowDialog() == true)
            {
                MessageBox.Show(addBorderDialog.BorderSettings.GreyLevel + " " + addBorderDialog.BorderSettings.Width);
                _currentlyOpenedPGM.PixelData = BorderPainter.AddBorder(
                    _currentlyOpenedPGM,
                    addBorderDialog.BorderSettings.Width,
                    addBorderDialog.BorderSettings.GreyLevel
                );
                renderedImage.Source = null;
                renderedImage.Source = _CreateBitmapFromPGM(_currentlyOpenedPGM);
                renderedImage.InvalidateVisual();
            }
        }

        private BitmapSource _CreateBitmapFromPGM(PGMImage img)
        {
            return BitmapSource.Create(
                pixelWidth: (int)img.Width,
                pixelHeight: (int)img.Height,
                dpiX: DPI,
                dpiY: DPI,
                pixelFormat: PixelFormats.Gray8,
                palette: null,
                pixels: img.PixelData,
                stride: (int)img.Width
            );
        }

        private void _UpdateMenuItemsClickableStatus(bool areEnabled)
        {
            this.saveMenuItem.IsEnabled = areEnabled;
            this.closeMenuItem.IsEnabled = areEnabled;
            this.borderMenuItem.IsEnabled = areEnabled;
        }
    }
}