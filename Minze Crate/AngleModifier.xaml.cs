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
        List<Game_Config> configs;
        Game_Config currentConfig;

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
        public void SetConfigs(List<Game_Config> conf)
        {
            if (conf == null) return;
            this.configs = conf;
            var comboContent = new ObservableCollection<string>();
            foreach (Game_Config config in this.configs)
            {
                comboContent.Add(config.game_name);
            }
            comboContent.Add("add new configuration");
            this.comboBox.ItemsSource = comboContent;
            this.comboBox.SelectedIndex = 0;
            this.currentConfig = this.configs[0];
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


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBox.SelectedItem == null) return;
            if (this.comboBox.SelectedItem.Equals("add new configuration"))
            {
                SelectName window = new SelectName();
                if (window.ShowDialog() == false)
                {
                    System.Windows.MessageBox.Show("Nothing");
                    var index = 0;
                    foreach (string item in this.comboBox.ItemsSource)
                    {
                        if (item.Equals(currentConfig.game_name))
                        {
                            this.comboBox.SelectedIndex = index;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(window.name);
                    if (configs.Find(conf => conf.game_name.Equals(window.name)) == null)
                    {

                        this.configs.Add(new Game_Config
                        {
                            game_name = window.name,
                            tilt = 69,
                            x1 = 30,
                            x2 = 50,
                            y1 = 38,
                            y2 = 87,
                            g = 255
                        });
                        var col = ((ObservableCollection<string>)this.comboBox.ItemsSource);
                        col.Remove("add new configuration");
                        col.Add(window.name);
                        col.Add("add new configuration");
                        this.comboBox.SelectedIndex = col.Count - 2;
                    }
                    else
                    {
                        var index = 0;
                        foreach (string item in this.comboBox.ItemsSource)
                        {
                            if (item.Equals(window.name))
                            {
                                this.comboBox.SelectedIndex = index;
                                break;
                            }
                            index++;
                        }

                    }
                    this.currentConfig = configs.Find(config => config.game_name.Equals(window.name));
                }
            }
            else
            {
                this.currentConfig = configs.Find(config => config.game_name.Equals(this.comboBox.SelectedItem));
                this.TiltSlider.Value = this.currentConfig.tilt;
                this.X1Slider.Value = this.currentConfig.x1;
                this.X2Slider.Value = this.currentConfig.x2;
                this.Y1Slider.Value = this.currentConfig.y1;
                this.Y2Slider.Value = this.currentConfig.y2;
                this.ledPicker.SelectedColor = new Color()
                {
                    R = this.currentConfig.r,
                    G = this.currentConfig.g,
                    B = this.currentConfig.b,
                    A = 255
                }; ;
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (currentConfig == null)
            {
                System.Windows.MessageBox.Show("Select a configuration first");
                return;
            }
            this.currentConfig.tilt = Convert.ToInt32(this.TiltSlider.Value);
            this.currentConfig.x1 = Convert.ToInt32(this.X1Slider.Value);
            this.currentConfig.x2 = Convert.ToInt32(this.X2Slider.Value);
            this.currentConfig.y1 = Convert.ToInt32(this.Y1Slider.Value);
            this.currentConfig.y2 = Convert.ToInt32(this.Y2Slider.Value);
            this.currentConfig.r = this.ledPicker.SelectedColor.Value.R;
            this.currentConfig.g = this.ledPicker.SelectedColor.Value.G;
            this.currentConfig.b = this.ledPicker.SelectedColor.Value.B;

            System.Windows.MessageBox.Show("Configuration saved");
        }
    }
}
