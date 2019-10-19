using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;
using System.Management;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using RestSharp;
using System.Collections.ObjectModel;

namespace Minze_Crate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Crate crate;
        List<Game_Config> game_Configs;
        Game_Config current_Game_Config;
        ArduinoPort selectedPort;
        List<ArduinoPort> ports;
        private double version = 0.2;
        public MainWindow()
        {
            InitializeComponent();
            this.setupDefaultGameConfigs();
            this.setupComboBox();
            this.RenderLayout();
            this.OnReload(null, null);
            AutoDetectUsb();
            Loaded += MainWindow_Loaded;
            this.CheckForNewVersionAsync();
        }
        private void CheckForNewVersionAsync()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var client = new RestClient("https://api.github.com");
            var request = new RestRequest("/repos/{user}/{repo}/releases/latest", Method.GET);
            request.AddUrlSegment("user", "minze25");
            request.AddUrlSegment("repo", "minze-crate");
            var response = client.Get(request);
            var ans = LatestRepoAnswer.FromJson(response.Content);
            if (Convert.ToDouble(ans.TagName) > this.version)
                MessageBox.Show("There is a new version, please update your program for maximum compatibility");

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //Login login = new Login();
            //login.ShowDialog();
        }

        private const string Passphrase = "r0iefcf27oe0fwog";
        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            ButtonSelector buttonSelector = new ButtonSelector();
            buttonSelector.SetFreeButtons(this.crate.free_buttons);
            if (buttonSelector.ShowDialog() == false) return;

            Button button = (Button)sender;
            string text = (string)button.Content;
            if (text.ToLower().IndexOf("none") >= 0 && buttonSelector.selectedButton == null) return;
            int pin;
            int.TryParse(button.Name.Substring(3), out pin);
            if (text.ToLower().IndexOf("none") < 0)
            {
                CrateButton crateBut = this.crate.used_buttons.Find(but => but.name.Equals(text));
                this.crate.FreeButton(crateBut);
            }
            if (buttonSelector.selectedButton != null)
            {

                this.crate.Usebutton(buttonSelector.selectedButton, pin);
                button.Content = buttonSelector.selectedButton.name;
            }
            this.RenderLayout();

        }
        public bool setCurrentGameConfig(Game_Config config)
        {
            if (!this.game_Configs.Contains(config)) return false;
            this.current_Game_Config = config;
            this.crate = this.current_Game_Config.crate;
            this.RenderLayout();
            return true;
        }
        private void ButtonActivator_Click(object sender, RoutedEventArgs e)
        {
            ButtonSelector buttonSelector = new ButtonSelector();
            buttonSelector.SetFreeButtons(Crate.createCrateWithButtons().used_buttons);
            if (buttonSelector.ShowDialog() == false) return;
            CrateButton but = buttonSelector.selectedButton;
            if (but == null)
            {
                this.ButtonActivator.Content = "None";
                this.current_Game_Config.activator = null;
                return;
            }
            this.ButtonActivator.Content = but.name;
            this.current_Game_Config.activator = but;
        }
        private void setupComboBox()
        {
            var col = new ObservableCollection<string>();
            foreach (var game_Config in this.game_Configs)
            {
                col.Add(game_Config.game_name);
            }
            col.Add("add new configuration");
            this.comboBox.ItemsSource = col;
            this.comboBox.SelectedIndex = 0;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.comboBox.SelectedItem == null) return;
            if (this.comboBox.SelectedItem.Equals("add new configuration"))
            {
                SelectName window = new SelectName();
                if (window.ShowDialog() == false)
                {
                    var index = 0;
                    foreach (string item in this.comboBox.ItemsSource)
                    {
                        if (item.Equals(this.current_Game_Config.game_name))
                        {
                            this.comboBox.SelectedIndex = index;
                            break;
                        }
                        index++;
                    }
                }
                else
                {
                    if (this.game_Configs.Find(conf => conf.game_name.Equals(window.name)) == null)
                    {

                        this.game_Configs.Add(new Game_Config
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
                    this.current_Game_Config = this.game_Configs.Find(config => config.game_name.Equals(window.name));
                }
            }
            else
            {
                this.setCurrentGameConfig(this.game_Configs.Find(config => config.game_name.Equals(this.comboBox.SelectedItem)));
                //this.current_Game_Config.crate.angles[0] = this.current_Game_Config.tilt;
                //this.current_Game_Config.crate.angles[1] = this.current_Game_Config.x1;
                //this.current_Game_Config.crate.angles[2] = this.current_Game_Config.x2;
                //this.current_Game_Config.crate.angles[3] = this.current_Game_Config.y1;
                //this.current_Game_Config.crate.angles[4] = this.current_Game_Config.y2;
                this.ledPicker.SelectedColor = new Color()
                {
                    R = this.current_Game_Config.r,
                    G = this.current_Game_Config.g,
                    B = this.current_Game_Config.b,
                    A = 255
                }; ;
            }
        }
        private void RenderLayout()
        {
            this.CleanAllButtons();
            foreach (CrateButton usedButton in this.crate.used_buttons)
            {
                var button = this.FindName("but" + usedButton.pin);
                if (button == null)
                {
                    continue;
                }
                ((Button)button).Content = usedButton.name;
            }
            this.ButtonActivator.Content = "None";
            if (this.current_Game_Config.activator != null)
                this.ButtonActivator.Content = this.current_Game_Config.activator.name;

        }
        private void CleanAllButtons()
        {
            for (int i = 10; i < 52; i++)
            {
                var button = this.FindName("but" + i);
                if (button != null)
                {
                    ((Button)button).Content = "None";
                }
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Middle || e.ButtonState != MouseButtonState.Pressed)
                return;
            Button button = (Button)sender;
            string text = (string)button.Content;
            if (text.ToLower().IndexOf("none") >= 0) return;
            CrateButton usedBut = this.crate.used_buttons.Find(but => but.name.Equals(text));
            this.crate.FreeButton(usedBut);
            button.Content = "None";
        }

        private void OnModifyAngles(object sender, RoutedEventArgs e)
        {
            AngleModifier window = new AngleModifier();
            window.SetAngles(this.crate.angles);
            window.currentConfig = this.current_Game_Config;
            if (window.ShowDialog() == false) return;
            this.crate.angles = window.GetAngles();
        }

        private void SaveConfiguration(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "config files (*.mnzbx)|*.mnzbx|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Game_Config>));
                TextWriter stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, this.game_Configs);
                string encrypted = Cypher.Encrypt(stringWriter.ToString(), Passphrase);
                using (BinaryWriter writer = new BinaryWriter(File.Open(saveFileDialog.FileName, FileMode.Create)))
                {
                    writer.Write(encrypted);
                }

            }
        }

        private void LoadConfiguration(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "config files (*.mnzbx)|*.mnzbx|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                //var fileStream = openFileDialog.OpenFile();
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    string encrypted = reader.ReadString();

                    var decrypted = Cypher.Decrypt(encrypted, Passphrase);
                    using (StringReader stringReader = new StringReader(decrypted))
                    {
                        //var fileContent = reader.ReadToEnd();
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Game_Config>));
                        List<Game_Config> newCrate = (List<Game_Config>)serializer.Deserialize(stringReader);
                        if (newCrate != null)
                        {
                            this.game_Configs = newCrate;
                            this.setCurrentGameConfig(this.game_Configs[0]);
                        }
                    }
                }
            }
        }

        private void OnReload(object sender, RoutedEventArgs e)
        {
            //foreach (var item in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
            //{
            //    MessageBox.Show(item);

            //}
            //System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MinZe_Crate.arduino-cli.exe")
            MenuItem reload = new MenuItem();
            reload.Header = "Reload";
            reload.Click += OnReload;

            var bw = new BackgroundWorker();
            bw.DoWork += (send, args) =>
            {
                this.ports =
                     ArduinoService.Instance.GetPorts();

            };
            bw.RunWorkerCompleted += (send, args) =>
            {
                this.PortSelector.Items.Clear();
                this.PortSelector.Items.Add(reload);
                this.PortSelector.Items.Add(new Separator());
                if (this.ports != null)
                {
                    foreach (ArduinoPort port in this.ports)
                    {

                        MenuItem item = new MenuItem();
                        item.Header = port.address + ":" + (port.boards != null ? port.boards[0].name : "unknown");
                        item.Click += OnSelectPort;
                        if (!this.PortSelector.Items.Contains(item))
                            this.PortSelector.Items.Add(item);

                    }
                }
            };
            bw.RunWorkerAsync();
        }

        private void OnSelectPort(object sender, RoutedEventArgs e)
        {
            MenuItem button = (MenuItem)sender;
            var text = button.Header.ToString();
            var portADD = text.Substring(0, text.IndexOf(":"));
            ArduinoPort ardPort = this.ports.Find(port => port.address.IndexOf(portADD) >= 0);
            if (ardPort == null) return;
            this.selectedPort = ardPort;
            this.PortSelector.Header = "Port: " + this.selectedPort.address;
        }

        private void AutoDetectUsb()
        {

            var watcher = new ManagementEventWatcher();
            var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent");
            watcher.EventArrived += Watcher_EventArrived; ;
            watcher.Query = query;
            watcher.Start();

        }

        private void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                this.OnReload(null, null);
            });
        }

        private void UploadToArduino(object sender, RoutedEventArgs e)
        {
            if (this.selectedPort == null)
                MessageBox.Show("Please, select a port");
            else
                MessageBox.Show(ArduinoService.Instance.sendToArduino(this.current_Game_Config.ToIno(), this.selectedPort));
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Program made by Aitor Garcia Diez\n" +
                "This is a program intented to be used with \n" +
                "the Box for smash that I build\n" +
                "Contact:\n" +
                "bbminze@gmail.com");
        }
        public void setupDefaultGameConfigs()
        {
            Game_Config def = new Game_Config
            {
                game_name = "PM",
                tilt = 69,
                x1 = 30,
                x2 = 50,
                y1 = 38,
                y2 = 87,
                r = 102,
                g = 77,
                b = 204,
                crate = Crate.createCrateWithButtons()

            };
            Game_Config melee = new Game_Config
            {
                game_name = "Melee",
                tilt = 74,
                x1 = 27,
                x2 = 55,
                y1 = 27,
                y2 = 53,
                r = 214,
                g = 104,
                b = 14,
                activator = CrateButton.X2,
                crate = Crate.createCrateWithButtons()

            };
            Game_Config ultimate = new Game_Config
            {
                game_name = "Ultimate",
                tilt = 69,
                x1 = 30,
                x2 = 50,
                y1 = 38,
                y2 = 87,
                r = 255,
                activator = CrateButton.Y2,
                crate = Crate.createCrateWithButtons()

            };
            this.game_Configs = new List<Game_Config>() { def, melee, ultimate };
            this.setCurrentGameConfig(def);
        }
    }
}
