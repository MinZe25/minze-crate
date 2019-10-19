using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minze_Crate
{
    public class Game_Config
    {
        public Game_Config()
        {
            activator = null;
            tilt = 0;
            x1 = 0;
            x2 = 0;
            y1 = 0;
            y2 = 0;
            r = 0;
            g = 0;
            b = 0;
            crate = Crate.createCrateWithButtons();
        }
        public string game_name;
        public bool analogShields = false;
        public int tilt;
        public int x1;
        public int x2;
        public int y1;
        public int y2;
        public byte r;
        public byte g;
        public byte b;
        public bool switchActive = false;
        public CrateButton activator;
        public Crate crate;
        public override string ToString()
        {
            string str = "class " + this.game_name + " : public game{public: " + this.game_name + "(){";
            str += "this->tilt =" + tilt + ";";
            str += "this->x1 =" + x1 + ";";
            str += "this->x2 =" + x2 + ";";
            str += "this->y1 =" + y1 + ";";
            str += "this->y2 =" + y2 + ";";
            str += "this->r =" + r + ";";
            str += "this->g =" + g + ";";
            str += "this->b =" + b + ";";
            if (analogShields) str += "this->analogShield = true;";
            str += "}};";
            return str;
        }
        public string ToIno()
        {
            string textPins = "";
            foreach (CrateButton item in this.crate.used_buttons)
            {
                textPins += "const int " + item.name + " = " + item.pin + ";";
            }
            foreach (CrateButton item in this.crate.free_buttons)
            {
                textPins += "const int " + item.name + " = " + item.pin + ";";
            }
            textPins += "const int X3v = " + this.crate.angles[0] + ";";
            textPins += "const int Y3v = " + this.crate.angles[0] + ";";
            textPins += "const int X1v = " + this.crate.angles[1] + ";";
            textPins += "const int X2v = " + this.crate.angles[2] + ";";
            textPins += "const int Y1v = " + this.crate.angles[3] + ";";
            textPins += "const int Y2v = " + this.crate.angles[4] + ";";
            textPins += "const int RLIGHTv = " + this.crate.angles[5] + ";";
            string textGames = "";
            textGames += "void chooseGame(){";
            string textClasses = "";

            textClasses += this.ToString();
            //if (this.activators.Count > 0)
            //{
            //    textGames += "if(";
            //    var first = true;
            //    foreach (CrateButton button in this.activators)
            //    {
            //        if (this.switchActive)
            //        {
            //            textGames += "digitalRead(" + CrateButton.SWITCH.name + ") == LOW && ";
            //        }
            //        if (first)
            //        {
            //            textGames += "digitalRead(" + button.name + ") == LOW";
            //            first = false;
            //            continue;
            //        }
            //        textGames += " && digitalRead(" + button.name + ")  == LOW";

            //    }
            //    textGames += ")";
            //}
            if (this.activator != null)
            {
                if (this.switchActive)
                {
                    textGames += "digitalRead(" + CrateButton.SWITCH.name + ") == LOW && ";
                }
                textGames += "digitalRead(" + this.activator.name + ") == LOW";
            }
            textGames += "currentGame = new " + this.game_name + "();";

            string text = this.crate.readResource("Minze_Crate.first_part.txt");
            text += textPins;
            text += textClasses;
            text += textGames;
            text += this.crate.readResource("Minze_Crate.second_part.txt");
            return text;
        }
    }
}
