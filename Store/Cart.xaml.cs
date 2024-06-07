using AntiqueShop.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            List<CartItems> cartItems = Connector.db.CartItems.Where(x => x.Users.user_id == MyID && x.order_id == null).ToList();

            ListCart.ItemsSource = cartItems;
        }   

        int MyID { get; set; }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int itemId = (int)button.Tag;

            CartItems cart = Connector.db.CartItems.FirstOrDefault(x => x.user_id == MyID && x.product_id == itemId && x.order_id == null);
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

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            List<CartItems> cartItems = Connector.db.CartItems.Where(x => x.Users.user_id == MyID && x.order_id == null).ToList();
            
            Orders order = new Orders()
            {
                user_id = MyID,
                order_date = DateTime.Now,
                status_id = 1
            };

            int orderId = order.order_id;
            int totalProducts = cartItems.Count;
            int overallStock = 0;
            decimal totalSum = 0.0M;

            foreach (var item in cartItems)
            {
                if (item.Products.stock < item.quantity)
                {
                    if (item.Products.stock <= 0)
                    {
                        Connector.db.CartItems.Remove(item);
                        MessageBox.Show($"Товара «{item.Products.name}» нет в наличии, он был убран из корзины.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        totalProducts--;
                    }
                    else
                    {
                        item.quantity = item.Products.stock;
                        MessageBox.Show($"Товара «{item.Products.name}» нет в таком количестве, количество было уменьшено до {item.Products.stock}.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        totalSum += item.Products.price * item.Products.stock;
                        item.Products.stock = 0;
                        overallStock += item.quantity;
                    }
                }
                else
                {
                    totalSum += item.Products.price * item.quantity;
                    item.Products.stock -= item.quantity;
                    overallStock += item.quantity;
                }

                item.order_id = orderId;
            }

            order.total_amount = totalSum;

            if (totalProducts == 0)
            {
                MessageBox.Show("Ваша корзина пуста.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Connector.db.Orders.Add(order);

            Connector.db.SaveChanges();
            
            MessageBox.Show("Заказ оформлен.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);

            ImageSource qr = Utils.Utils.GenerateQR($"ord:{order.order_id}-itm:{overallStock}-ttl:{totalSum}");
            
            InitializeData();

            CreatePDF(order.order_id);
            AppFrame.MainFrame.Navigate(new QRViewer(qr));
        }

        private void CreatePDF(int orderId)
        {
            Document doc = new Document();

            PdfWriter.GetInstance(doc, new FileStream($"..\\..\\PDF\\{orderId}.pdf", FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);

            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph($"Заказ № {orderId}", font);
            paragraph.Alignment = Element.ALIGN_CENTER;
            doc.Add(paragraph);

            decimal totalSum = 0.0M;

            List<CartItems> cartItems = Connector.db.CartItems.Where(x => x.Users.user_id == MyID && x.order_id == orderId).ToList();

            foreach (var item in cartItems)
            {
                iTextSharp.text.Paragraph itemParagraph = new iTextSharp.text.Paragraph($"{item.Products.name} — {item.quantity} шт. — {item.Products.price * item.quantity} руб.", font);
                itemParagraph.Alignment = Element.ALIGN_LEFT;
                doc.Add(itemParagraph);

                totalSum += item.Products.price * item.quantity;
            }

            iTextSharp.text.Paragraph totalParagraph = new iTextSharp.text.Paragraph($"\n\nИтого: {totalSum} руб.", font);
            doc.Add(totalParagraph);
            
            doc.Close();
            Process.Start($"..\\..\\PDF\\{orderId}.pdf");
        }
    }
}
