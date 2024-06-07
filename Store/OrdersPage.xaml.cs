using AntiqueShop.Models;
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

namespace AntiqueShop.Store
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage(int id)
        {
            MyID = id;
            InitializeComponent();
            InitializeData();
        }

        public int MyID { get; set; }

        private void InitializeData()
        {
            Users user = Connector.db.Users.FirstOrDefault(x => x.user_id == MyID);

            if (user.role_id == 1)
            {
                List<Orders> orders = Connector.db.Orders.Where(x => x.user_id == MyID).ToList();
                ListOrders.ItemsSource = orders;
            }
            else
            {
                List<Orders> orders = Connector.db.Orders.ToList();
                ListOrders.ItemsSource = orders;
            }
        }

        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            int orderId = (int)(sender as Button).Tag;

            AppFrame.MainFrame.Navigate(new OrderView(orderId, MyID));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeData();
        }
    }
}
