using SuperiorHackBase.Core;
using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwnAdventure3Hax.Hax
{
    public class PwnieHax04_PointerChain : PwnieHax
    {
        private static Pointer[] POSITION_CHAIN = new Pointer[] { /* TODO: Figure out! */ };

        private Vector3 currentPosition = Vector3.Zero;

        protected override void OnTick(TimeSpan delta)
        {
            base.OnTick(delta);

            var positionAddress = this.Memory.ResolvePointerChain(MainModule.BaseAddress, POSITION_CHAIN); //Resolve the pointer chain
            var newPosition = this.Memory.Read<Vector3>(positionAddress); //Read the current position
            if (currentPosition != newPosition) //If the position changed...
            {
                currentPosition = newPosition; //... save its new value ...
                Console.WriteLine("{0}\t{1}\t{2}", //... and print it!
                    (int)currentPosition.X, (int)currentPosition.Y, (int)currentPosition.Z);
            }
        }
    }
}
