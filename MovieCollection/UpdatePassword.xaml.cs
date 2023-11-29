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
using System.Windows.Shapes;
using DataObjects;
using LogicLayer;

namespace MovieCollection
{
    /// <summary>
    /// Interaction logic for UpdatePassword.xaml
    /// </summary>
    public partial class UpdatePassword : Window
    {
        string _email = null;
        public UpdatePassword(string email)
        {
            InitializeComponent();
            _email = email;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text != _email)
            {
                MessageBox.Show("Update Failed", "Password not changed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (pwdNewPassword.Password.ToString() == "password" || pwdNewPassword.Password.ToString() == pwdOldPassword.ToString())
            {
                MessageBox.Show("You must use a new password.", "Reenter Passwords", MessageBoxButton.OK, MessageBoxImage.Information);
                pwdNewPassword.Password = "";
                pwdRetypePassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }
            if (pwdNewPassword.Password != pwdRetypePassword.Password)
            {
                MessageBox.Show("Password do not match.", "Reenter Passwords", MessageBoxButton.OK, MessageBoxImage.Information);
                pwdNewPassword.Password = "";
                pwdRetypePassword.Password = "";
                pwdNewPassword.Focus();
                return;
            }
            if (!pwdNewPassword.Password.IsValidPassword())
            {
                MessageBox.Show("Password Too Short", "Password not changed", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            string oldPassword = pwdOldPassword.Password;
            string newPassword = pwdNewPassword.Password;
            string retypePassword = pwdRetypePassword.Password;

            UserManager userManager = new UserManager();
            try
            {
                if (userManager.ResetPassword(_email, oldPassword, newPassword) == true)
                {
                    this.DialogResult = true;

                }
                else
                {
                    this.DialogResult = false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            btnSubmit.IsDefault = true;
            txtEmail.Focus();
        }
    }
}
