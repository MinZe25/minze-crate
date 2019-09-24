using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Minze_Crate
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            Closing += Login_Closing;
        }

        private void Login_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string sMessageBoxText = "Do you want to exit?";
            string sCaption = "My Test Application";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;

                default:
                    e.Cancel = true;
                    break;
            }

        }

        private void OnLogin(object sender, RoutedEventArgs e)
        {
            if (userNameInput.Text.Length < 3)
            {
                MessageBox.Show("Invalid username format");
                return;
            }
            if (passwordInput.Password.Length < 3)
            {
                MessageBox.Show("Invalid password format");
                return;
            }
            WebRequest request = WebRequest.Create("api.aitordev.com/box/login");
            request.Headers.Add("username", this.userNameInput.Text);
            request.Headers.Add("hash_pass", this.passwordInput.Password);
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            var serializer = new DataContractJsonSerializer(typeof(PostActionResult));
            var result = serializer.ReadObject(response.GetResponseStream())
                             as PostActionResult;
            if (result.Value.ToLower().IndexOf("ok") < 0)
            {
                MessageBox.Show("Login failed");
            }
            else
                Close();

        }
        public class PostActionResult
        {
            public string Value { get; set; }
        }

    }
}
