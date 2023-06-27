using Rul.Entities;
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

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderTicketPage.xaml
    /// </summary>
    public partial class OrderTicketPage : Page
    {
        List<Product> productList = new List<Product>();
        public OrderTicketPage(Order currentOrder , List<Product> products)
        {
            InitializeComponent();

            productList = products;
            DataContext = currentOrder;


            try { 
            txtPickupPoint.Text = currentOrder.PickupPoint.Address; //Выводим адрес пункта выдачи
            
            var result = "";
            foreach (var pl in productList)
                result += (result == "" ? "" : ", ") + pl.ProductName.ToString(); // Выводим название товаров, оформленных в заказе
            txtProductList.Text = result.ToString();

            var total = productList.Sum(p => Convert.ToDouble(p.ProductCost) - Convert.ToDouble(p.ProductCost) * Convert.ToDouble(p.ProductDiscountAmount / 100.00));
            txtCost.Text = total.ToString() + "Рублей"; // Выводим итоговую сумму заказа

            var discountSum = productList.Sum(p => p.ProductDiscountAmount);
            txtDiscountSum.Text = discountSum.ToString() + "%";// Выводим итогувую сумму заказа
            } catch (Exception e) {
                MessageBox.Show("Введите данные");
            }
        }

        private void btnSaveDocument_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if(pd.ShowDialog() == true)
            {
                IDocumentPaginatorSource idp = flowDoc;
                pd.PrintDocument(idp.DocumentPaginator, Title);
            }
        }
    }
}
