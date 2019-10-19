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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minze_Crate
{
    /// <summary>
    /// Interaction logic for SelectName.xaml
    /// </summary>
    public partial class SelectName : Window
    {
        public string name;
        public SelectName()
        {
            InitializeComponent();
        }

        private void UserNameInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            this.name = userNameInput.Text;
            this.DialogResult = true;
            this.Close();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.name = userNameInput.Text;
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
