﻿using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using PGMViewer.Common;
using PGMViewer.Models;

namespace PGMViewer.UI
{
    public partial class MainWindow : Window
    {
        private const string DEFAULT_TITLE = "PGM Viewer";

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
                    _currentlyOpenedPGM.Name = openFileDialog.SafeFileName;
                    renderedImage.Source = _currentlyOpenedPGM.ToBitmapSource();
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
            saveFileDialog.Filter = "PGM (P2)|*.pgm|Bitmap Picture|*.bmp|JPEG image|*.jpg|PNG image|*.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                _currentlyOpenedPGM.Save(saveFileDialog.FileName);
            }
        }

        private void OnAddBorder_Click(object sender, RoutedEventArgs e)
        {
            var addBorderDialog = new AddBorderDialog(_currentlyOpenedPGM.Width);
            if (addBorderDialog.ShowDialog() == true)
            {
                _currentlyOpenedPGM.PixelData = BorderPainter.AddBorder(
                    _currentlyOpenedPGM,
                    addBorderDialog.BorderSettings.Width,
                    addBorderDialog.BorderSettings.GreyLevel
                );

                renderedImage.Source = null;
                renderedImage.Source = _currentlyOpenedPGM.ToBitmapSource();
                renderedImage.InvalidateVisual();
            }
        }

        private void _UpdateMenuItemsClickableStatus(bool areEnabled)
        {
            this.saveMenuItem.IsEnabled = areEnabled;
            this.closeMenuItem.IsEnabled = areEnabled;
            this.borderMenuItem.IsEnabled = areEnabled;
        }
    }
}