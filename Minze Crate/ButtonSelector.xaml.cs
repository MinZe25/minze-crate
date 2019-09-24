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
namespace Minze_Crate
{
    /// <summary>
    /// Interaction logic for ButtonSelector.xaml
    /// </summary>
    public partial class ButtonSelector : Window
    {
        public CrateButton selectedButton = null;
        public List<CrateButton> buttons_list;
        public ButtonSelector()
        {
            InitializeComponent();
        }
        public void SetFreeButtons(List<CrateButton> buttons)
        {
            this.buttons_list = buttons;
            foreach (CrateButton free_button in buttons)
            {
                Button button = new Button();
                button.Click += this.Button_Click;
                button.Content = free_button.name;
                button.Height = 40;
                this.buttonPanel.Children.Add(button);
            }
        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string text = (string)button.Content;
            if (text.ToLower().IndexOf("none") >= 0)

                this.selectedButton = null;
            else
                this.selectedButton = buttons_list.Find(but => but.name.Equals(text));
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
