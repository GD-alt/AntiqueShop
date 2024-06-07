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
    /// Логика взаимодействия для OrderView.xaml
    /// </summary>
    public partial class OrderView : Page
    {
        public OrderView(int orderId, int id)
        {
            InitializeComponent();
            MyID = id;
            OrderID = orderId;
            InitializeData();
        }

        int MyID { get; set; }
        int OrderID { get; set; }
        Dictionary<string, int> statuses = new Dictionary<string, int>();

        private void InitializeData()
        {
            List<OrderStatuses> orderStatuses = Connector.db.OrderStatuses.ToList();

            foreach (var item in orderStatuses)
            {
                if (statuses.ContainsKey(item.status_name))
                {
                    continue;
                }

                statuses.Add(item.status_name, item.status_id);
            }

            Users users = Connector.db.Users.FirstOrDefault(x => x.user_id == MyID);
            Orders order = Connector.db.Orders.FirstOrDefault(x => x.order_id == OrderID);

            List<CartItems> cartItems = Connector.db.CartItems.Where(x => x.order_id == OrderID).ToList();

            if (users.role_id == 1)
            {
                StatusCombo.IsEnabled = false;
                DelBtn.Visibility = Visibility.Hidden;
            }

            StatusCombo.ItemsSource = statuses.Keys;
            StatusCombo.SelectedItem = statuses.FirstOrDefault(x => x.Value == order.status_id).Key;

            ListCart.ItemsSource = cartItems;
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите удалить заказ {OrderID}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Orders order = Connector.db.Orders.FirstOrDefault(x => x.order_id == OrderID);
                Connector.db.Orders.Remove(order);

                foreach (var item in Connector.db.CartItems.Where(x => x.order_id == OrderID))
                {
                    Connector.db.CartItems.Remove(item);
                }

                Connector.db.SaveChanges();
                AppFrame.MainFrame.GoBack();
            }
        }

        private void StatusCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Orders order = Connector.db.Orders.FirstOrDefault(x => x.order_id == OrderID);
            order.status_id = statuses[StatusCombo.SelectedItem.ToString()];
            Connector.db.SaveChanges();
            InitializeData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.GoBack();
        }
    }
}
