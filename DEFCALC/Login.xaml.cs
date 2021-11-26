using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DEFCALC
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Вход в систему
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntLogin_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtLogin.Text.Trim()) && !String.IsNullOrEmpty(txtPassword.Password.Trim()))
            {
                App.userKey = "111111";

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Application.Current.MainWindow = mainWindow;
                this.Close();
            }
            else
            {
                lblWarning.Content = "Введите учетные данные";
            }
        }
    }
}
