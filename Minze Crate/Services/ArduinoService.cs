using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Minze_Crate
{
    class Board
    {
        public string name;
        public string FQBN;
    }
    class ArduinoPort
    {
        public string address;
        public string protocol;
        public string protocol_label;
        public Board[] boards;
    }
    class ArduinoAdapter
    {
        public List<ArduinoPort> ports;
    }
    class ArduinoService
    {
        private string cliPath;
        private string cliFolder;
        private static ArduinoService instance;
        public static ArduinoService Instance {
            get {
                if (instance == null) instance = new ArduinoService();
                return instance;
            }
            private set { }
        }
        private ArduinoService()
        {
            this.cliFolder = this.cliPath = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "MinZe_Crate");
            this.cliPath = Path.Combine(this.cliFolder, "arduino-cli.exe");
            CopyCliToAppdata();
        }
        public string sendToArduino(string code, ArduinoPort port)
        {
            string folderPath = CreateIno(code);
            CLI("compile --fqbn arduino:avr:mega " + folderPath);
            return CLI("upload --fqbn arduino:avr:mega -p " + port.address + " " + folderPath);
        }

        private string CreateIno(string code)
        {
            string ticks = DateTime.Now.Ticks.ToString();
            string filename = ticks + ".ino";
            string folderPath = Path.Combine(this.cliFolder, ticks);
            System.IO.Directory.CreateDirectory(folderPath);
            string fullFilename = Path.Combine(folderPath, filename);
            using (FileStream fileStream = new FileStream(fullFilename, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(code);
                    return folderPath;
                }
            }
        }

        private string CLI(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = this.cliPath;
            process.StartInfo.Arguments = command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            var output = process.StandardOutput.ReadToEnd();
            //byte[] bytes = Encoding.Default.GetBytes(output);
            //output = Encoding.UTF8.GetString(bytes);
            return output;
        }
        private void CopyCliToAppdata()
        {
            if (!File.Exists(this.cliFolder)) System.IO.Directory.CreateDirectory(this.cliFolder);
            if (File.Exists(this.cliPath)) return;
            using (FileStream fileStream = new FileStream(cliPath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = "Minze_Crate.arduino-cli.exe";

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        byte[] data = StreamToBytes(stream);
                        binaryWriter.Write(data);
                    }
                }
            }

        }
        public List<ArduinoPort> GetPorts()
        {
            if (!CoresAreInstalled())
                UpdateAndDownloadCores();
            var jsonString = CLI("board list --format json");
            if (jsonString.Length < 5) return null;
            JsonSerializer serializer = new JsonSerializer();
            TextReader reader = new StringReader(jsonString);
            JsonReader jsonReader = new JsonTextReader(reader);
            ArduinoAdapter ports = serializer.Deserialize<ArduinoAdapter>(jsonReader);

            return ports.ports.Distinct().ToList();
        }
        public Boolean UploadToPort(string port)
        {
            return true;
        }
        private Boolean CoresAreInstalled()
        {
            var jsonString = (CLI("core list --format json"));

            JArray jObject = JArray.Parse(jsonString);
            foreach (JObject item in jObject)
            {

                foreach (JProperty property in item.Properties())
                {
                    var name = property.Name;
                    var value = (string)property.Value;
                    if (name.IndexOf("ID") >= 0 && value.IndexOf("arduino:avr") >= 0) return true;
                }
            }
            return false;

        }
        private void UpdateAndDownloadCores()
        {

            CLI("core update-index");
            CLI("core install arduino:avr");
            CLI("lib install Nintendo");
        }
        private byte[] StreamToBytes(Stream input)
        {

            int capacity = input.CanSeek ? (int)input.Length : 0; //Bitwise operator - If can seek, Capacity becomes Length, else becomes 0.
            using (MemoryStream output = new MemoryStream(capacity)) //Using the MemoryStream output, with the given capacity.
            {
                int readLength;
                byte[] buffer = new byte[capacity/*4096*/];  //An array of bytes
                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);   //Read the memory data, into the buffer
                    output.Write(buffer, 0, readLength); //Write the buffer to the output MemoryStream incrementally.
                }
                while (readLength != 0); //Do all this while the readLength is not 0
                return output.ToArray();  //When finished, return the finished MemoryStream object as an array.
            }

        }

    }
}
