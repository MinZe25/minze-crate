using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minze_Crate.Models
{
    class CrateButtonComparer : IEqualityComparer<CrateButton>
    {
        public bool Equals(CrateButton x, CrateButton y)
        {
            return x.name == y.name;
        }

        public int GetHashCode(CrateButton obj)
        {
            return obj.GetHashCode();
        }
    }
}
