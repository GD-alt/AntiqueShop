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
using AntiqueShop.Store;
using AntiqueShop.Utils;

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
            string[] data = { LgnBox.Text, PasswdBox.Password };
            string lgn = LgnBox.Text;
            Users user = Connector.db.Users.FirstOrDefault(x => x.email == lgn);

            if (Utils.Utils.AnyIsNullOrEmpty(data))

            if (user == null)
            {
                MessageBox.Show("Taкогo пользователя нет!", "Oшибка авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Utils.Encoder.ValidateString(PasswdBox.Password, user.password))
            {
                MessageBox.Show("Неверный пароль!", "Oшибка авторизации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Добро пожаловать, {user.full_name}!", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            AppFrame.MainFrame.Navigate(new Store.Store(user.role_id, user.user_id));
        }
    }
}
