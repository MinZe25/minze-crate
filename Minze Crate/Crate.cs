using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Minze_Crate
{

    public class Crate
    {
        public List<CrateButton> free_buttons;
        public List<CrateButton> used_buttons;
        public List<Game_Config> configs;
        public double[] angles = { 71, 27, 55, 27, 53, 74 };
        public Boolean FreeButton(CrateButton button)
        {
            if (button == null) return false;
            if (!used_buttons.Contains(button)) return false;
            if (free_buttons.Contains(button)) return false;
            button.pin = 0;
            used_buttons.Remove(button);
            free_buttons.Add(button);
            return true;
        }
        public Boolean Usebutton(CrateButton button, int pin)
        {
            if (button == null) return false;
            if (!free_buttons.Contains(button)) return false;
            if (used_buttons.Contains(button)) return false;
            button.pin = pin;
            free_buttons.Remove(button);
            used_buttons.Add(button);
            return true;
        }
        public string ToIno()
        {
            string textPins = "";
            foreach (CrateButton item in used_buttons)
            {
                textPins += "const int " + item.name + " = " + item.pin + ";";
            }
            foreach (CrateButton item in free_buttons)
            {
                textPins += "const int " + item.name + " = " + item.pin + ";";
            }
            textPins += "const int X3v = " + this.angles[0] + ";";
            textPins += "const int Y3v = " + this.angles[0] + ";";
            textPins += "const int X1v = " + this.angles[1] + ";";
            textPins += "const int X2v = " + this.angles[2] + ";";
            textPins += "const int Y1v = " + this.angles[3] + ";";
            textPins += "const int Y2v = " + this.angles[4] + ";";
            textPins += "const int RLIGHTv = " + this.angles[5] + ";";
            string textGames = "";
            textGames += "void chooseGame(){";
            string textClasses = "";
            foreach (Game_Config config in this.configs)
            {
                textClasses += config.ToString();
                if (config.activators.Count > 0)
                {
                    textGames += "if(";
                    var first = true;
                    foreach (CrateButton button in config.activators)
                    {
                        if (first)
                        {
                            textGames += "digitalRead(" + button.name + ") == LOW";
                            first = false;
                            continue;
                        }
                        textGames += " && digitalRead(" + button.name + ")  == LOW";

                    }
                    textGames += ")";
                }
                textGames += "currentGame = new " + config.game_name + "();";
            }
            string text = readResource("Minze_Crate.first_part.txt");
            text += textPins;
            text += textClasses;
            text += textGames;
            text += readResource("Minze_Crate.second_part.txt");
            return text;
        }
        private string readResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
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
                b = 204
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
                activators = new List<CrateButton>()
                {
                    CrateButton.SWITCH,CrateButton.X2
                }
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
                activators = new List<CrateButton>()
                {
                    CrateButton.SWITCH,CrateButton.Y2
                }
            };
            this.configs.Add(def);
            this.configs.Add(melee);
            this.configs.Add(ultimate);

        }
        public Crate()
        {
            this.used_buttons = new List<CrateButton>();
            this.free_buttons = new List<CrateButton>();
            this.configs = new List<Game_Config>();
            this.used_buttons.Add(CrateButton.A);
            this.used_buttons.Add(CrateButton.B);
            this.used_buttons.Add(CrateButton.X);
            this.used_buttons.Add(CrateButton.Y);
            this.used_buttons.Add(CrateButton.Z);
            this.used_buttons.Add(CrateButton.START);
            this.used_buttons.Add(CrateButton.R);
            this.used_buttons.Add(CrateButton.L);
            this.used_buttons.Add(CrateButton.RLIGHT);
            this.used_buttons.Add(CrateButton.LEFT);
            this.used_buttons.Add(CrateButton.RIGHT);
            this.used_buttons.Add(CrateButton.UP);
            this.used_buttons.Add(CrateButton.DOWN);
            this.used_buttons.Add(CrateButton.X1);
            this.used_buttons.Add(CrateButton.X2);
            this.used_buttons.Add(CrateButton.Y1);
            this.used_buttons.Add(CrateButton.Y2);
            this.used_buttons.Add(CrateButton.Tilt);
            this.used_buttons.Add(CrateButton.CLEFT);
            this.used_buttons.Add(CrateButton.CRIGHT);
            this.used_buttons.Add(CrateButton.CUP);
            this.used_buttons.Add(CrateButton.CDOWN);
            this.used_buttons.Add(CrateButton.SWITCH);
        }
    }
}
