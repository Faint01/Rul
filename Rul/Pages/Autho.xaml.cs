using Rul.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        private int countUnsuccessful = 0;
        public Autho()
        {
            InitializeComponent();

            txtCaptcha.Visibility = Visibility.Hidden;
            textBlockCaptcha.Visibility = Visibility.Hidden;
        }
        //Login: loginDErfg2018
        //Password: 5ovb1N
        
        private void GenerateCaptcha()
        {
            txtCaptcha.Visibility = Visibility.Visible;
            textBlockCaptcha.Visibility = Visibility.Visible;

            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = " ";
            string temp = " ";
            Random r = new Random();

            for (int i = 0; i < 6; i++)

            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }

            textBlockCaptcha.Text = pwd;
           // textBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
        }
     
        private void LoadForm(string _role, User user)
        {
            switch ( _role )
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Менеджер":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Администратор":
                    NavigationService.Navigate(new Admin(user));
                    break;
            }
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            User user = new User();

            user = RulEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).FirstOrDefault();
            int userCount = RulEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).Count();

            if (countUnsuccessful < 1)
            {
                if (userCount > 0)
                {
                    MessageBox.Show("Вы вошли под " + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(), user);
                }
                else
                {
                    MessageBox.Show("Вы ввели неверно логин или пароль");
                    countUnsuccessful++;
                    if (countUnsuccessful == 1)
                        GenerateCaptcha();
                }
            }
            else
            {
                if (txtCaptcha.Text == textBlockCaptcha.Text)
                {
                    MessageBox.Show("Вы вошли под" + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(), user);
                }
                else
                {
                    MessageBox.Show("Введите данные заново");
                }
            }
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null)); //Переход на станицу клиента для неавторизованного пользователя
        }
    }
}
