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

namespace AntiqueShop.Store
{
    /// <summary>
    /// Логика взаимодействия для UserEdit.xaml
    /// </summary>
    public partial class UserEdit : Page
    {
        public Dictionary<string, int> roles = new Dictionary<string, int>();

        public UserEdit(int id)
        {
            InitializeComponent();
            UserID = id;

            Users user = Connector.db.Users.Find(id);
            List<Roles> rolesList = Connector.db.Roles.ToList();

            for (int i = 0; i < rolesList.Count; i++)
            {
                if (roles.Keys.ToList().Contains(rolesList[i].role_name))
                    continue;

                roles.Add(rolesList[i].role_name, rolesList[i].role_id);
                RoleCombo.Items.Add(rolesList[i].role_name);
            }

            NameBox.Text = user.first_name;
            SurnameBox.Text = user.last_name;
            PhoneBox.Text = user.phone;
            MailBox.Text = user.email;
            RoleCombo.SelectedIndex = roles[user.Roles.role_name] - 1;
        }

        public int UserID { get; set; }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.GoBack();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validators.ValidateEmail(MailBox.Text))
            {
                MessageBox.Show("Некорректный email.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Validators.ValidatePhone(PhoneBox.Text))
            {
                MessageBox.Show("Некорректный номер телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Users user = Connector.db.Users.Find(UserID);

            user.first_name = NameBox.Text;
            user.last_name = SurnameBox.Text;
            user.email = MailBox.Text;
            user.phone = PhoneBox.Text;
            user.role_id = roles[RoleCombo.SelectedItem.ToString()];

            Connector.db.SaveChanges();
            AppFrame.MainFrame.GoBack();
        }
    }
}
