using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tiler
{
    public partial class TilerWindow : Window
    {
        private static readonly int TILE_SIZE = 512;

        private FileInfo fileToTile;

        public TilerWindow()
        {
            InitializeComponent();

            this.fileToTile = null;
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true && openFileDialog.CheckPathExists)
            {
                fileToTile = new FileInfo(openFileDialog.FileName);

                this.txtEditor.Text = openFileDialog.FileName;
            }
        }

        private void btnLaunchTile_Click(object sender, RoutedEventArgs e)
        {
            this.btnLaunchTile.IsEnabled = false;

            Task.Factory.StartNew(() => this.CreateFolder());
        }

        private void CreateFolder()
        {
            if (this.fileToTile != null && this.fileToTile.Exists)
            {
                string lImageName = System.IO.Path.GetFileNameWithoutExtension(fileToTile.Name);

                DirectoryInfo directory = new DirectoryInfo(this.fileToTile.DirectoryName + System.IO.Path.DirectorySeparatorChar + lImageName);

                if (directory.Exists)
                {
                    directory.Delete(true);
                }

                directory.Create();
                FileInfo header = new FileInfo(directory.FullName + System.IO.Path.DirectorySeparatorChar + lImageName + ".header");

                Bitmap image = new Bitmap(fileToTile.FullName);
                float width = image.Width;
                float height = image.Height;

                int nbTimes = (int) Math.Min(Math.Ceiling(width / TILE_SIZE), Math.Ceiling(height / TILE_SIZE));
                int nbZoom = (int) Math.Log(nbTimes, 2);

                string headerContent = width + ":" + height;
                File.WriteAllText(header.FullName, headerContent);

                Dispatcher.Invoke(() =>
                {
                    this.prgBar.Value = 0;
                });

                for(int i = 0; i < nbZoom; i++)
                {
                    DirectoryInfo subDirectory = new DirectoryInfo(directory.FullName + System.IO.Path.DirectorySeparatorChar + (i + 1));
                    subDirectory.Create();

                    Bitmap subImage = this.CreateBitmapFrom(image, i);

                    this.FragmentImageTo(subImage, subDirectory.FullName);

                    // To remove
                    // subImage.Save(subDirectory.FullName + System.IO.Path.DirectorySeparatorChar + "full.jpg", ImageFormat.Jpeg);

                    if (subImage != image)
                    {
                        subImage.Dispose();
                    }

                    Dispatcher.Invoke(() =>
                    {
                        this.prgBar.Value = ((i + 1) * 100d) / (nbZoom);
                    });
                }

                image.Dispose();
            }

            Dispatcher.Invoke(() =>
            {
                this.btnLaunchTile.IsEnabled = true;
            });
        }

        private Bitmap CreateBitmapFrom(Bitmap source, int nbZoom)
        {
            if(nbZoom == 0)
            {
                return source;
            }

            int ratio = (int)Math.Pow(2, nbZoom);
            int width = source.Width / ratio;
            int height = source.Height / ratio;

            Bitmap subImage = new Bitmap(width, height);


            for(int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    subImage.SetPixel(j, i, source.GetPixel(j * ratio, i * ratio));
                }
            }

            return subImage;
        }

        private void FragmentImageTo(Bitmap image, string directoryPath)
        {
            int centerX = image.Width / 2;
            int centerY = image.Height / 2;

            int nbFragmentX = (int)Math.Ceiling((decimal)centerX / TILE_SIZE);
            int nbFragmentY = (int)Math.Ceiling((decimal)centerY / TILE_SIZE);

            // Corner left-top
            for (int y = 0; y < nbFragmentY; y++)
            {
                for (int x = 0; x < nbFragmentX; x++)
                {
                    Bitmap fragImage = new Bitmap(TILE_SIZE, TILE_SIZE);

                    int coordX = centerX - (x + 1) * TILE_SIZE;
                    int coordY = centerY - (y + 1) * TILE_SIZE;

                    for (int i = 0; i < fragImage.Height; i++)
                    {
                        for (int j = 0; j < fragImage.Width; j++)
                        {
                            if(coordX + j > 0 && coordY + i > 0
                                && coordX + j < image.Width && coordY + i < image.Height)
                            {
                                fragImage.SetPixel(j, i, image.GetPixel(coordX + j, coordY + i));
                            }
                        }
                    }

                    fragImage.Save(directoryPath + System.IO.Path.DirectorySeparatorChar + -(x + 1) + "," + -(y + 1) + ".jpg", ImageFormat.Jpeg);

                    fragImage.Dispose();
                }
            }

            // Corner right-top
            for (int y = 0; y < nbFragmentY; y++)
            {
                for (int x = 0; x < nbFragmentX; x++)
                {
                    Bitmap fragImage = new Bitmap(TILE_SIZE, TILE_SIZE);

                    int coordX = centerX + x * TILE_SIZE;
                    int coordY = centerY - (y + 1) * TILE_SIZE;

                    for (int i = 0; i < fragImage.Height; i++)
                    {
                        for (int j = 0; j < fragImage.Width; j++)
                        {
                            if (coordX + j > 0 && coordY + i > 0
                                && coordX + j < image.Width && coordY + i < image.Height)
                            {
                                fragImage.SetPixel(j, i, image.GetPixel(coordX + j, coordY + i));
                            }
                        }
                    }

                    fragImage.Save(directoryPath + System.IO.Path.DirectorySeparatorChar + x + "," + -(y + 1) + ".jpg", ImageFormat.Jpeg);

                    fragImage.Dispose();
                }
            }

            // Corner left-bottom
            for (int y = 0; y < nbFragmentY; y++)
            {
                for (int x = 0; x < nbFragmentX; x++)
                {
                    Bitmap fragImage = new Bitmap(TILE_SIZE, TILE_SIZE);

                    int coordX = centerX - (x + 1) * TILE_SIZE;
                    int coordY = centerY + y * TILE_SIZE;

                    for (int i = 0; i < fragImage.Height; i++)
                    {
                        for (int j = 0; j < fragImage.Width; j++)
                        {
                            if (coordX + j > 0 && coordY + i > 0
                                && coordX + j < image.Width && coordY + i < image.Height)
                            {
                                fragImage.SetPixel(j, i, image.GetPixel(coordX + j, coordY + i));
                            }
                        }
                    }

                    fragImage.Save(directoryPath + System.IO.Path.DirectorySeparatorChar + -(x + 1) + "," + y + ".jpg", ImageFormat.Jpeg);

                    fragImage.Dispose();
                }
            }

            // Corner right-bottom
            for (int y = 0; y < nbFragmentY; y++)
            {
                for (int x = 0; x < nbFragmentX; x++)
                {
                    Bitmap fragImage = new Bitmap(TILE_SIZE, TILE_SIZE);

                    int coordX = centerX + x * TILE_SIZE;
                    int coordY = centerY + y * TILE_SIZE;

                    for (int i = 0; i < fragImage.Height; i++)
                    {
                        for (int j = 0; j < fragImage.Width; j++)
                        {
                            if (coordX + j > 0 && coordY + i > 0
                                && coordX + j < image.Width && coordY + i < image.Height)
                            {
                                fragImage.SetPixel(j, i, image.GetPixel(coordX + j, coordY + i));
                            }
                        }
                    }

                    fragImage.Save(directoryPath + System.IO.Path.DirectorySeparatorChar + x + "," + y + ".jpg", ImageFormat.Jpeg);

                    fragImage.Dispose();
                }
            }
        }
    }
}
