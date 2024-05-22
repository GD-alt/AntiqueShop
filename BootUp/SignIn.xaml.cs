using System;
using System.Collections.Generic;
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
using AntiqueShop.Models;

namespace AntiqueShop.BootUp
{
    /// <summary>
    /// Логика взаимодействия для SignIn.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void SgnupBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new SignUp());
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            Users user = Connector.db.Users.FirstOrDefault(x => x.email == LgnBox.Text);

            if (user == null)
            {
                MessageBox.Show("Taкогo пользователя нет! ", "Oшибка авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
