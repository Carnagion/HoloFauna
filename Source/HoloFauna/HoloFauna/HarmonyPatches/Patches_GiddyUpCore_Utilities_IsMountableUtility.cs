using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoloFauna
{
    public static class Patches_GiddyUpCore_Utilities_IsMountableUtility
    {
        /// <summary>
        /// prevents holo animals from appearing in mod options and being used as mounts
        /// </summary>
        public static bool Postfix_isAllowedInModOptions(bool originalBool, string animalName)
        {
            if (animalName.Contains("HoloFauna_Holo"))
            {
                originalBool = false;
            }
            return originalBool;
        }
    }
}
