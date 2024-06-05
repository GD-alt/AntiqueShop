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

namespace AntiqueShop.Store
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public UsersPage(int id)
        {
            ID = id;
            InitializeComponent();

            List<Users> users = Connector.db.Users.ToList();
            ListUsers.ItemsSource = users;
        }

        public int ID { get; set; }

        public void UpdateData()
        {
            List<Users> users = Connector.db.Users.ToList();
            ListUsers.ItemsSource = users;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Console.WriteLine((int)button.Tag);
            Console.WriteLine(ID);

            if ((int)button.Tag == ID)
            {
                MessageBox.Show("Вы не можете редактировать свой аккаунт.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            AppFrame.MainFrame.Navigate(new UserEdit((int)button.Tag));
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if ((int)button.Tag == ID)
            {
                MessageBox.Show("Вы не можете удалить свой аккаунт. зачем", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Users user = Connector.db.Users.Find((int)button.Tag);

            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя {user.full_name} (ID {user.user_id})?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Connector.db.Users.Remove(user);
                Connector.db.SaveChanges();
                UpdateData();
            }
        }
    }
}
