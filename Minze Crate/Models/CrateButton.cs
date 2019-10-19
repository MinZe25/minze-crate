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
        public CrateButton clone()
        {
            return new CrateButton(this.name, this.pin);
        }
        public static readonly CrateButton A = new CrateButton("A", 22);
        public static readonly CrateButton B = new CrateButton("B", 24);
        public static readonly CrateButton X = new CrateButton("X", 26);
        public static readonly CrateButton Y = new CrateButton("Y", 28);
        public static readonly CrateButton Z = new CrateButton("Z", 30);
        public static readonly CrateButton START = new CrateButton("START", 31);
        public static readonly CrateButton R = new CrateButton("R", 34);
        public static readonly CrateButton L = new CrateButton("L", 35);
        public static readonly CrateButton RLIGHT = new CrateButton("RLIGHT", 36);
        public static readonly CrateButton LEFT = new CrateButton("LEFT", 38);
        public static readonly CrateButton RIGHT = new CrateButton("RIGHT", 39);
        public static readonly CrateButton UP = new CrateButton("UP", 40);
        public static readonly CrateButton DOWN = new CrateButton("DOWN", 41);
        public static readonly CrateButton X1 = new CrateButton("X1", 44);
        public static readonly CrateButton X2 = new CrateButton("X2", 45);
        public static readonly CrateButton Y1 = new CrateButton("Y1", 46);
        public static readonly CrateButton Y2 = new CrateButton("Y2", 47);
        public static readonly CrateButton Tilt = new CrateButton("Tilt", 11);
        public static readonly CrateButton CLEFT = new CrateButton("CLEFT ", 48);
        public static readonly CrateButton CRIGHT = new CrateButton("CRIGHT", 49);
        public static readonly CrateButton CUP = new CrateButton("CUP   ", 50);
        public static readonly CrateButton CDOWN = new CrateButton("CDOWN ", 51);
        public static readonly CrateButton SWITCH = new CrateButton("SWITCH", 12);
    }

}
