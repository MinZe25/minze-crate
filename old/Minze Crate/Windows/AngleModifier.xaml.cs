using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.Toolkit;
namespace Minze_Crate
{
    /// <summary>
    /// Interaction logic for AngleModifier.xaml
    /// </summary>
    public partial class AngleModifier : Window
    {
        public Game_Config currentConfig;
        public AngleModifier()
        {
            InitializeComponent();
        }
        public double[] GetAngles()
        {
            return new double[] {
                this.TiltSlider.Value,
                this.X1Slider.Value,
                this.X2Slider.Value,
                this.Y1Slider.Value,
                this.Y2Slider.Value,
                this.LightShieldSlider.Value
            };
        }
        public Boolean SetAngles(double[] angles)
        {
            if (angles.Length != 6) return false;
            this.TiltSlider.Value = angles[0];
            this.X1Slider.Value = angles[1];
            this.X2Slider.Value = angles[2];
            this.Y1Slider.Value = angles[3];
            this.Y2Slider.Value = angles[4];
            this.LightShieldSlider.Value = angles[5];
            return true;
        }
        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            this.currentConfig.tiltValue = Convert.ToInt32(this.TiltSlider.Value);
            this.currentConfig.x1Value = Convert.ToInt32(this.X1Slider.Value);
            this.currentConfig.x2Value = Convert.ToInt32(this.X2Slider.Value);
            this.currentConfig.y1Value = Convert.ToInt32(this.Y1Slider.Value);
            this.currentConfig.y2Value = Convert.ToInt32(this.Y2Slider.Value);
            System.Windows.MessageBox.Show("Configuration saved");
        }

        
    }
}
