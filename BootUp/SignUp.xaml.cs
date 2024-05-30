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
using AntiqueShop.Utils;

namespace AntiqueShop.BootUp
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new SignIn());
            Products product = new Products();
        }

        private void SgnupBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] data = { LgnBox.Text, PasswdBox.Password, RPasswdBox.Password, NameBox.Text, SurnameBox.Text, PhoneBox.Text, MailBox.Text };
            if (Utils.Utils.AnyIsNullOrEmpty(data))
            {
                MessageBox.Show("Поля не должны быть пусты!", "Oшибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Validators.ValidateEmail(MailBox.Text))
            {
                MessageBox.Show("Неверная почта!", "Oшибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Validators.ValidatePhone(PhoneBox.Text))
            {
                MessageBox.Show("Неверный номер!", "Oшибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Users user1 = Connector.db.Users.FirstOrDefault(x => x.email == MailBox.Text);
            Users user2 = Connector.db.Users.FirstOrDefault(x => x.phone == PhoneBox.Text);

            if (user1 != null ||  user2 != null)
            {
                MessageBox.Show("Аккаунт с такими данными уже есть!", "Oшибка регистрации!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Users user = new Users()
            {
                first_name = NameBox.Text,
                last_name = SurnameBox.Text,
                role_id = 1,
                phone = PhoneBox.Text,
                email = MailBox.Text
            };
            

            Connector.db.Users.Add(user);
            Connector.db.SaveChanges();
        }
    }
}
