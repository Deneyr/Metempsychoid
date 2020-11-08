using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    public partial class OpenFileDialogSample : Window
    {
        private FileInfo fileToTile;

        public OpenFileDialogSample()
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
            
        }
    }
}
