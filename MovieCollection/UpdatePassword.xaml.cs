using DataObjects;
using LogicLayer;
using System;
using System.Windows;

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
