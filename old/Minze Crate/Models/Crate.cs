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
        public double[] angles = { 71, 27, 55, 27, 53, 74 };
        public bool FreeButton(CrateButton button)
        {
            if (button == null) return false;
            if (!used_buttons.Contains(button)) return false;
            if (free_buttons.Contains(button)) return false;
            button.pin = 0;
            used_buttons.Remove(button);
            free_buttons.Add(button);
            return true;
        }
        public bool Usebutton(CrateButton button, int pin)
        {
            if (button == null) return false;
            if (!this.free_buttons.Contains(button)) return false;
            if (this.used_buttons.Contains(button)) return false;
            button.pin = pin;
            this.free_buttons.Remove(button);
            this.used_buttons.Add(button);
            return true;
        }

    

        public Crate()
        {
            this.used_buttons = new List<CrateButton>();
            this.free_buttons = new List<CrateButton>();
        }
        public static Crate createCrateWithButtons()
        {
            var crate = new Crate();
            crate.used_buttons.Add(CrateButton.A.clone());
            crate.used_buttons.Add(CrateButton.B.clone());
            crate.used_buttons.Add(CrateButton.X.clone());
            crate.used_buttons.Add(CrateButton.Y.clone());
            crate.used_buttons.Add(CrateButton.Z.clone());
            crate.used_buttons.Add(CrateButton.START.clone());
            crate.used_buttons.Add(CrateButton.R.clone());
            crate.used_buttons.Add(CrateButton.L.clone());
            crate.used_buttons.Add(CrateButton.RLIGHT.clone());
            crate.used_buttons.Add(CrateButton.LEFT.clone());
            crate.used_buttons.Add(CrateButton.RIGHT.clone());
            crate.used_buttons.Add(CrateButton.UP.clone());
            crate.used_buttons.Add(CrateButton.DOWN.clone());
            crate.used_buttons.Add(CrateButton.X1.clone());
            crate.used_buttons.Add(CrateButton.X2.clone());
            crate.used_buttons.Add(CrateButton.Y1.clone());
            crate.used_buttons.Add(CrateButton.Y2.clone());
            crate.used_buttons.Add(CrateButton.Tilt.clone());
            crate.used_buttons.Add(CrateButton.CLEFT.clone());
            crate.used_buttons.Add(CrateButton.CRIGHT.clone());
            crate.used_buttons.Add(CrateButton.CUP.clone());
            crate.used_buttons.Add(CrateButton.CDOWN.clone());
            crate.used_buttons.Add(CrateButton.SWITCH.clone());
            return crate;
        }
    }
}
