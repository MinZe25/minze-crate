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
            activators = new List<CrateButton>();
            tilt = 0;
            x1 = 0;
            x2 = 0;
            y1 = 0;
            y2 = 0;
            r = 0;
            g = 0;
            b = 0;
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
        public List<CrateButton> activators;
        public override string ToString()
        {
            string str = "class " + this.game_name + " : public game{public: " +this.game_name+"(){";
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
    }
}
