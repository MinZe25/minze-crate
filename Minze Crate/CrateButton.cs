using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minze_Crate
{
    public class CrateButton
    {
        public string name;
        public int pin;
        public CrateButton()
        {

        }
        public CrateButton(string name, int pin)
        {
            this.name = name;
            this.pin = pin;
        }
        public static CrateButton A = new CrateButton("A", 22);
        public static CrateButton B = new CrateButton("B", 24);
        public static CrateButton X = new CrateButton("X", 26);
        public static CrateButton Y = new CrateButton("Y", 28);
        public static CrateButton Z = new CrateButton("Z", 30);
        public static CrateButton START = new CrateButton("START", 31);
        public static CrateButton R = new CrateButton("R", 34);
        public static CrateButton L = new CrateButton("L", 35);
        public static CrateButton RLIGHT = new CrateButton("RLIGHT", 36);
        public static CrateButton LEFT = new CrateButton("LEFT", 38);
        public static CrateButton RIGHT = new CrateButton("RIGHT", 39);
        public static CrateButton UP = new CrateButton("UP", 40);
        public static CrateButton DOWN = new CrateButton("DOWN", 41);
        public static CrateButton X1 = new CrateButton("X1", 44);
        public static CrateButton X2 = new CrateButton("X2", 45);
        public static CrateButton Y1 = new CrateButton("Y1", 46);
        public static CrateButton Y2 = new CrateButton("Y2", 47);
        public static CrateButton Tilt = new CrateButton("Tilt", 11);
        public static CrateButton CLEFT = new CrateButton("CLEFT ", 48);
        public static CrateButton CRIGHT = new CrateButton("CRIGHT", 49);
        public static CrateButton CUP = new CrateButton("CUP   ", 50);
        public static CrateButton CDOWN = new CrateButton("CDOWN ", 51);
        public static CrateButton SWITCH = new CrateButton("SWITCH", 12);
    }


}
