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
        public Store()
        {
            InitializeComponent();
            InitializeData();
        }

        List<Sizes> sizes = Connector.db.Sizes.ToList();
        bool sortOrder = false;
        string size = "";

        public void InitializeData()
        {
            List<Products> products = Connector.db.Products.ToList();
            products = products.OrderBy(x => x.price).ToList();
            ListProducts.ItemsSource = products;

            SortCombo.Items.Add("Возр.");
            SortCombo.Items.Add("Убыв.");
            SortCombo.SelectedIndex = 0;

            FilterCombo.Items.Add("Все");

            for (int i = 0; i < sizes.Count; i++)
            {
                FilterCombo.Items.Add(sizes[i].size_name);
            }

            FilterCombo.SelectedIndex = 0;
        }

        public void Update()
        {
            List<Products> products = Connector.db.Products.ToList();

            products = products.Where(x => x.description.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            if (size != "")
                products = products.Where(x => x.Sizes.size_name.ToLower().Equals(size.ToLower())).ToList();

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
                size = "";
            }
            else
            {
                size = item;
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
    }
}
