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

namespace Minze_Crate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Crate crate;
        ArduinoPort selectedPort;
        List<ArduinoPort> ports;
        private double version = 0.1;
        public MainWindow()
        {
            InitializeComponent();
            this.crate = new Crate();
            this.crate.setupDefaultGameConfigs();
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
            window.SetConfigs(this.crate.configs);
            if (window.ShowDialog() == false) return;
            this.crate.angles = window.GetAngles();
        }

        private void SaveConfiguration(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "config files (*.mnzbx)|*.mnzbx|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Crate));
                TextWriter stringWriter = new StringWriter();
                serializer.Serialize(stringWriter, this.crate);
                stringWriter.ToString();
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
                        XmlSerializer serializer = new XmlSerializer(typeof(Crate));
                        Crate newCrate = (Crate)serializer.Deserialize(stringReader);
                        if (newCrate != null)
                        {
                            this.crate = newCrate;
                            this.RenderLayout();
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
                MessageBox.Show(ArduinoService.Instance.sendToArduino(this.crate.ToIno(), this.selectedPort));
        }

        private void OnAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Program made by Aitor Garcia Diez\n" +
                "This is a program intented to be used with \n" +
                "the Box for smash that I build\n" +
                "Contact:\n" +
                "bbminze@gmail.com");
        }
    }
}
