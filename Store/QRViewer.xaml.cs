using AntiqueShop.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace AntiqueShop.Store
{
    /// <summary>
    /// Логика взаимодействия для QRViewer.xaml
    /// </summary>
    public partial class QRViewer : Page
    {
        public QRViewer(ImageSource bitmap)
        {
            InitializeComponent();
            QRImage.Source = bitmap;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.GoBack();
        }
    }
}
