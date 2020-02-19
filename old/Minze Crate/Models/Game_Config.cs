using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Minze_Crate
{
    public class Game_Config
    {
        public Game_Config()
        {
            activator = null;
            tiltValue = 0;
            x1Value = 0;
            x2Value = 0;
            y1Value = 0;
            y2Value = 0;
            r = 0;
            g = 0;
            b = 0;
            crate = Crate.createCrateWithButtons();
        }
        public string game_name;
        public bool analogShields = false;
        public int tiltValue;
        public int x1Value;
        public int x2Value;
        public int y1Value;
        public int y2Value;
        public int lightShieldValue;
        public byte r;
        public byte g;
        public byte b;
        public bool switchActive = false;
        public CrateButton activator;
        public Crate crate;
        public override string ToString()
        {
            var gameName = this.game_name.Replace(" ", "_");
            string str = "class " + gameName + " : public game{public: " + gameName + "(){";
            str += "this->tilt =" + tiltValue + ";";
            str += "this->x1 =" + x1Value + ";";
            str += "this->x2 =" + x2Value + ";";
            str += "this->y1 =" + y1Value + ";";
            str += "this->y2 =" + y2Value + ";";
            str += "this->r =" + r + ";";
            str += "this->g =" + g + ";";
            str += "this->b =" + b + ";";
            str += "this->lightShieldValue =" + lightShieldValue + ";";
            foreach (CrateButton item in this.crate.used_buttons)
            {
                str += "this->" + item.name + " = " + item.pin + ";";
            }
            foreach (CrateButton item in this.crate.free_buttons)
            {
                str += "this->" + item.name + " = " + item.pin + ";";
            }
            if (analogShields) str += "this->analogShield = true;";
            str += "}};";
            return str;
        }
    }
}
