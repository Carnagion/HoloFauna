using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoloFauna
{
    public class CompCannotExist : ThingComp
    {
        /// <summary>
        /// destroys thing
        /// </summary>
        public override void CompTick()
        {
            base.CompTick();
            this.parent.Destroy(DestroyMode.Vanish);
        }

        /// <summary>
        /// destroys thing
        /// </summary>
        public override void CompTickRare()
        {
            base.CompTickRare();
            this.parent.Destroy(DestroyMode.Vanish);
        }

        /// <summary>
        /// destroys thing
        /// </summary>
        public override void CompTickLong()
        {
            base.CompTickLong();
            this.parent.Destroy(DestroyMode.Vanish);
        }
    }
}
