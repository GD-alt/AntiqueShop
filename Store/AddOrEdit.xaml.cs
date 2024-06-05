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
        public Dictionary<string, int> sizes = new Dictionary<string, int>();
        public Dictionary<string, int> categories = new Dictionary<string, int>();
        public Dictionary<string, int> colors = new Dictionary<string, int>();
        public List<string> imagesList = new List<string>();

        public AddOrEdit(bool editMode, int itemId)
        {
            InitializeComponent();

            EditMode = editMode;
            ProductId = itemId;

            // Init dictionary for sizes where key is size name and value is size id
            List<Sizes> sizesList = Connector.db.Sizes.ToList();

            for (int i = 0; i < sizesList.Count; i++)
            {
                sizes.Add(sizesList[i].size_name, sizesList[i].size_id);
                SizeCombo.Items.Add(sizesList[i].size_name);
            }

            // For categories
            List<Categories> categoriesList = Connector.db.Categories.ToList();

            for (int i = 0; i < categoriesList.Count; i++)
            {
                categories.Add(categoriesList[i].category_name, categoriesList[i].category_id);
                CatCombo.Items.Add(categoriesList[i].category_name);
            }

            // For colors
            List<Models.Colors> colorsList = Connector.db.Colors.ToList();

            for (int i = 0; i < colorsList.Count; i++)
            {
                colors.Add(colorsList[i].color_name, colorsList[i].color_id);
                ColorCombo.Items.Add(colorsList[i].color_name);
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
                SizeCombo.SelectedIndex = sizesList.IndexOf(sizesList.Where(x => x.size_id == product.size_id).FirstOrDefault());
                ColorCombo.SelectedIndex = colorsList.IndexOf(colorsList.Where(x => x.color_id == product.color_id).FirstOrDefault());
            }
            else
            {
                CatCombo.SelectedIndex = 0;
                ImgCombo.SelectedIndex = 0;
                SizeCombo.SelectedIndex = 0;
                ColorCombo.SelectedIndex = 0;
                Title = "Добавление товара";
                AddButton.Content = "Добавить";
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
            if (NameBox.Text == "" || DescBox.Text == "" || PriceBox.Text == "" || InStockBox.Text == "" || SizeCombo.SelectedIndex == -1 || ColorCombo.SelectedIndex == -1 || CatCombo.SelectedIndex == -1 || ImgCombo.SelectedIndex == -1)
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
            product.size_id = sizes[SizeCombo.SelectedItem.ToString()];
            product.color_id = colors[ColorCombo.SelectedItem.ToString()];

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
    }
}
