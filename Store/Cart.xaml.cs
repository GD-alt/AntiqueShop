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
    /// Логика взаимодействия для Cart.xaml
    /// </summary>
    public partial class Cart : Page
    {
        public Cart(int id)
        {
            InitializeComponent();
            MyID = id;
            InitializeData();
        }

        private void InitializeData()
        {
            List<CartItems> cartItems = Connector.db.CartItems.Where(x => x.Users.user_id == MyID).ToList();

            ListCart.ItemsSource = cartItems;
        }   

        int MyID { get; set; }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int itemId = (int)button.Tag;

            CartItems cart = Connector.db.CartItems.FirstOrDefault(x => x.user_id == MyID && x.product_id == itemId);
            Products product = Connector.db.Products.FirstOrDefault(x => x.product_id == itemId);

            if (cart.quantity < product.stock)
            {
                cart.quantity++;
                MessageBox.Show("Товар добавлен в корзину.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Товара нет в наличии.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                if (product.stock <= 0)
                {
                    Connector.db.CartItems.Remove(cart);
                }
                else
                {
                    cart.quantity = product.stock;
                }
            }

            Connector.db.SaveChanges();
            InitializeData();
        }

        private void ReduceBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int itemId = (int)button.Tag;

            CartItems cart = Connector.db.CartItems.FirstOrDefault(x => x.user_id == MyID && x.product_id == itemId && x.order_id == null);
            Products product = Connector.db.Products.FirstOrDefault(x => x.product_id == itemId);

            if (cart.quantity > 1)
            {
                if (product.stock <= 0)
                {
                    Connector.db.CartItems.Remove(cart);
                }
                else
                {
                    cart.quantity--;
                }

                MessageBox.Show("Товар удален из корзины.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Connector.db.CartItems.Remove(cart);
                MessageBox.Show("Товар удален из корзины.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            Connector.db.SaveChanges();
            InitializeData();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.GoBack();
        }
    }
}
