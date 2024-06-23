using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddOrEdit.xaml
    /// </summary>
    public partial class AddOrEdit : Page
    {
        public Dictionary<string, int> categories = new Dictionary<string, int>();
        public List<string> imagesList = new List<string>();

        public AddOrEdit(bool editMode, int itemId)
        {
            InitializeComponent();

            EditMode = editMode;
            ProductId = itemId;

            // For categories
            List<Categories> categoriesList = Connector.db.Categories.ToList();

            for (int i = 0; i < categoriesList.Count; i++)
            {
                if (categories.Keys.Contains(categoriesList[i].category_name))
                   continue;

                categories.Add(categoriesList[i].category_name, categoriesList[i].category_id);
                CatCombo.Items.Add(categoriesList[i].category_name);
            }

            List<Products> productsList = Connector.db.Products.ToList();

            imagesList.Add("placeholder.jpg");
            ImgCombo.Items.Add("placeholder.jpg");

            for (int i = 0; i < productsList.Count; i++)
            {
                if (imagesList.Contains(productsList[i].image_url))
                    continue;

                imagesList.Add(productsList[i].image_url);
                ImgCombo.Items.Add(productsList[i].image_url);
            }

            if (editMode)
            {
                AddButton.Content = "Сохранить";
                Products product = Connector.db.Products.Find(itemId);
                NameBox.Text = product.name;
                DescBox.Text = product.description;
                PriceBox.Text = product.price.ToString();
                CatCombo.SelectedIndex = product.category_id - 1;
                ImgCombo.SelectedIndex = imagesList.IndexOf(product.image_url);
                InStockBox.Text = product.stock.ToString();
            }
            else
            {
                CatCombo.SelectedIndex = 0;
                ImgCombo.SelectedIndex = 0;
                Title = "Добавление товара";
                AddButton.Content = "Добавить";
                DelButton.Visibility = Visibility.Hidden;
            }
        }

        public bool EditMode { get; set; }
        public int ProductId { get; set; }

        private void DescBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[^0-9,]"))
            {
                e.Handled = true;
            }
        }

        private void InStockBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[^0-9]"))
            {
                e.Handled = true;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppFrame.MainFrame.GoBack();
        }

        private bool ValidateInputs()
        {
            if (NameBox.Text == "" || DescBox.Text == "" || PriceBox.Text == "" || InStockBox.Text == "" || LifetimeBox.Text == "" || WeightBox.Text == "" || CatCombo.SelectedIndex == -1 || ImgCombo.SelectedIndex == -1)
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }

            // If price is decimal that failed to parse, return error
            if (!decimal.TryParse(PriceBox.Text, out decimal price))
            {
                MessageBox.Show("Цена должна быть числом!");
                return false;
            }

            if (price < 0)
            {
                MessageBox.Show("Цена не может быть меньше 0!");
                return false;
            }

            // If stock is int that failed to parse, return error
            if (!int.TryParse(InStockBox.Text, out int stock))
            {
                MessageBox.Show("Количество на складе должно быть числом!");
                return false;
            }

            // If stock is int that failed to parse, return error
            if (!int.TryParse(WeightBox.Text, out int weight))
            {
                MessageBox.Show("Вес должен быть числом!");
                return false;
            }

            // If stock is int that failed to parse, return error
            if (!int.TryParse(LifetimeBox.Text, out int lifetime))
            {
                MessageBox.Show("Срок годности должен быть числом!");
                return false;
            }

            if (!Regex.IsMatch(PriceBox.Text, @"[0-9]+\,[0-9]{2}"))
            {
                MessageBox.Show("Неверный формат числа!\nПример: 10,00");
                return false;
            }

            // Id stock is less than 0, return error
            if (stock < 0)
            {
                MessageBox.Show("Количество на складе не может быть меньше 0!");
                return false;
            }

            return true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
                return;

            Products product = new Products();

            if (EditMode)
            {
                product = Connector.db.Products.Find(ProductId);
            }

            product.name = NameBox.Text;
            product.description = DescBox.Text;
            product.price = decimal.Parse(PriceBox.Text);
            product.category_id = categories[CatCombo.SelectedItem.ToString()];
            product.image_url = imagesList[ImgCombo.SelectedIndex];
            product.stock = int.Parse(InStockBox.Text);
            product.shelf_life_days = int.Parse(LifetimeBox.Text);
            product.weight_grams = int.Parse(WeightBox.Text);
            product.nutrition_id = 1;
            product.packaging_id = 1;

            // If edit mode, update product
            if (EditMode)
            {
                Connector.db.SaveChanges();
            }
            else
            {
                Connector.db.Products.Add(product);
                Connector.db.SaveChanges();
            }

            AppFrame.MainFrame.GoBack();
        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            Products product = Connector.db.Products.Find(ProductId);

            if (MessageBox.Show($"Вы уверены, что хотите удалить товар {product.name} (ID {product.product_id})?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Connector.db.Products.Remove(product);
                Connector.db.SaveChanges();
                AppFrame.MainFrame.GoBack();
            }
        }

        private void WeightCombo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (Regex.IsMatch(e.Text, @"[^0-9]"))
            {
                e.Handled = true;
            }
        }

        private void LifetimeBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (Regex.IsMatch(e.Text, @"[^0-9]"))
            {
                e.Handled = true;
            }
        }
    }
}
