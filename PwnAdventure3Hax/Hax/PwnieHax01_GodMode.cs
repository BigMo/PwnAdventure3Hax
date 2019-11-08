using SuperiorHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwnAdventure3Hax.Hax
{
    public class PwnieHax01_GodMode : PwnieHax
    {
        private static Pointer HEALTH_ADDRESS = new Pointer(0x00000000); //TODO: Figure out!

        /// <summary>
        /// This method is called periodically (each 10ms by default)
        /// </summary>
        /// <param name="delta">The time passed since the last exxecution</param>
        protected override void OnTick(TimeSpan delta)
        {
            base.OnTick(delta);

            this.Memory.Write(HEALTH_ADDRESS, 999);
        }
    }
}
