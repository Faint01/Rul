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
using Rul.Pages;
using Rul.Windows;

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        User user = new User();
        public Admin(User currentUser)
        {
            InitializeComponent();

            var product = RulEntities.GetContext().Product.ToList();
            LViewProduct.ItemsSource = product;
            DataContext = this; // Привязываем контекст к коду , чтобы обратится к массивам

            txtAllAmount.Text = product.Count().ToString(); //Передаем количество свех записей из таблицы

            user = currentUser; //Передаем конкретного пользователя в пустой объект

            UpdateData(); // Вызываем метод
            User(); // Вызываем метод
        }
        private void User()
        {
            if (user != null)
                txtFullname.Text = user.UserSurname.ToString() + user.UserName.ToString() + " " + user.UserPatronymic.ToString();
            else
                txtFullname.Text = "Гость";
        }

        public string[] SortingList { get; set; } =
        {
            "Без сортировки",
            "Стоимость по возрастанию",
            "Стоимость по убыванию"
        };

        public string[] FilterList { get; set; } =
        {
            "Все диапазоны",
            "0%-9,99%",
            "10%-14,99%",
            "15% и более"
        };

        private void UpdateData()
        {
            var result = RulEntities.GetContext().Product.ToList();

            if (cmbSorting.SelectedIndex == 1)                          //Реализация сортировки
                result = result.OrderBy(p => p.ProductCost).ToList();   // C помощью запросов на сортировку по возрастанию
            if (cmbSorting.SelectedIndex == 2)                          // И убыванию цены     
                result = result.OrderByDescending(p => p.ProductCost).ToList();

            if (cmbFilter.SelectedIndex == 1)
                result = result.Where(p => p.ProductDiscountAmount >= 0 && p.ProductDiscountAmount < 10).ToList();
            if (cmbFilter.SelectedIndex == 2)
                result = result.Where(p => p.ProductDiscountAmount >= 10 && p.ProductDiscountAmount < 15).ToList();
            if (cmbFilter.SelectedIndex == 3)
                result = result.Where(p => p.ProductDiscountAmount >= 15).ToList();

            result = result.Where(p => p.ProductName.ToLower().Contains(txtSearch.Text.ToLower())).ToList(); //Реализация поиска
            LViewProduct.ItemsSource = result;

            txtResultAmount.Text = result.Count().ToString(); // Передаем количество людей после применения поиска , сортировки и фильтрации
        }

        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void cmbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateData();
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        List<Product> orderProduct = new List<Product>();
        private void btnAddProduct_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            orderProduct.Add(LViewProduct.SelectedItem as Product);

            if (orderProduct.Count > 0)
            {
                btnOrder.Visibility = Visibility.Visible;
            }
        }

        private void btnOrder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OrderWinow order = new OrderWinow(orderProduct, user);
        }

        private void LViewProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddNewProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(null));
        }

        private void LViewProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new AddEditProductPage(LViewProduct.SelectedItem as Product));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                RulEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                LViewProduct.ItemsSource = RulEntities.GetContext().Product.ToList();
            }
        }
    }
}
