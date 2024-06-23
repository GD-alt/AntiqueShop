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
    /// Логика взаимодействия для Store.xaml
    /// </summary>
    public partial class Store : Page
    {
        public Store(int role, int id)
        {
            InitializeComponent();
            Role = role;

            switch (Role)
            {
                case 1:
                    AddGood.Visibility = Visibility.Hidden;
                    EditGood.Visibility = Visibility.Hidden;
                    UsersMenu.Visibility = Visibility.Hidden;
                    break;
                case 2:
                    UsersMenu.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }

            InitializeData();
            MyID = id;
        }

        public int Role { get; set; }
        public int MyID { get; set; }

        List<Categories> categories = Connector.db.Categories.ToList();
        bool sortOrder = false;
        string category = "";
        int selectedItem = 0;
        bool onlySelected = false;

        public void InitializeData()
        {
            List<Products> products = Connector.db.Products.ToList();
            products = products.OrderBy(x => x.price).ToList();
            ListProducts.ItemsSource = products;

            SortCombo.Items.Add("Возр.");
            SortCombo.Items.Add("Убыв.");
            SortCombo.SelectedIndex = 0;

            FilterCombo.Items.Add("Все");

            for (int i = 0; i < categories.Count; i++)
            {
                FilterCombo.Items.Add(categories[i].category_name);
            }

            FilterCombo.SelectedIndex = 0;
        }

        public void Update()
        {
            List<Products> products = Connector.db.Products.ToList();

            if (onlySelected)
                products = Connector.db.Favourites.Where(x => x.Users.user_id == MyID).Select(x => x.Products).ToList();

            products = products.Where(x => x.description.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            if (category != "")
                products = products.Where(x => x.Categories.category_name.ToLower().Equals(category.ToLower())).ToList();

            if (sortOrder)
            {
                products = products.OrderByDescending(x => x.price).ToList();
            }
            else
            {
                products = products.OrderBy(x => x.price).ToList();
            }

            ListProducts.ItemsSource = products;
        }

        private void FilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string item = FilterCombo.SelectedItem.ToString();

            if (item == "Все")
            {
                category = "";
            }
            else
            {
                category = item;
            }

            Update();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortCombo.SelectedIndex == 0)
            {
                sortOrder = false;
            }
            else
            {
                sortOrder = true;
            }

            Update();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void ListProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < e.AddedItems.Count; i++)
            {
                Products item = (Products)e.AddedItems[i];
                selectedItem = item.product_id;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вышли из системы.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            AppFrame.MainFrame.Navigate(new BootUp.SignIn());
        }

        private void EditGood_Click(object sender, RoutedEventArgs e)
        {
            if (selectedItem == 0)
            {
                return;
            }

            AppFrame.MainFrame.Navigate(new AntiqueShop.Store.AddOrEdit(true, selectedItem));
        }

        private void AddGood_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new AntiqueShop.Store.AddOrEdit(false, 0));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Update();
        }

        private void UsersMenu_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new AntiqueShop.Store.UsersPage(MyID));
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int itemId = (int)button.Tag;

            CartItems cart = Connector.db.CartItems.FirstOrDefault(x => x.user_id == MyID && x.product_id == itemId && x.order_id == null);
            Products product = Connector.db.Products.FirstOrDefault(x => x.product_id == itemId);

            if (cart != null) {
                if (cart.quantity < product.stock)
                {
                    cart.quantity++;
                    MessageBox.Show("Товар добавлен в корзину.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    Connector.db.SaveChanges();
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
            }
            else
            {
                if (product.stock > 0)
                {
                    CartItems newCart = new CartItems
                    {
                        user_id = MyID,
                        product_id = itemId,
                        quantity = 1
                    };

                    Connector.db.CartItems.Add(newCart);
                    MessageBox.Show("Товар добавлен в корзину.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {   
                    MessageBox.Show("Товара нет в наличии.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Connector.db.SaveChanges();
            Console.WriteLine("done");
        }

        private void CartBtn_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new AntiqueShop.Store.Cart(MyID));
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.Navigate(new AntiqueShop.Store.OrdersPage(MyID));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            return; // not implemented

            onlySelected = ChBox.IsChecked.Value;

            Update();
        }

        private void AddFavourite_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int itemId = (int)button.Tag;

            Favourites fav = Connector.db.Favourites.FirstOrDefault(x => x.user_id == MyID && x.product_id == itemId);

            if (fav != null)
            {
                Connector.db.Favourites.Remove(fav);

                Connector.db.SaveChanges();

                MessageBox.Show("Товар удален из избранного.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Favourites newFav = new Favourites
                {
                    user_id = MyID,
                    product_id = itemId,
                    added_date = DateTime.Now
                };

                Connector.db.Favourites.Add(newFav);
                Connector.db.SaveChanges();
                MessageBox.Show("Товар добавлен в избранное.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ChBox_Click(object sender, RoutedEventArgs e)
        {
            onlySelected = ChBox.IsChecked.Value;

            Update();
        }
    }
}
