using SuperiorHackBase.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwnAdventure3Hax.Hax
{
    public class PwnieHax03_Teleport : PwnieHax
    {
        private static Pointer POSITION_ADDRESS = new Pointer(0x00000000); //TODO: Figure out!

        protected override void OnKeyDown(Keys key)
        {
            base.OnKeyDown(key);

            /* TODO:
             * 1) Get player's current position
             * 2) Upon NumPad1 being pressed, save the current position (aka save position to teleport to)
             * 3) Upon NumPad2 being pressed, restore the current position! (aka teleport to saved position)
             * 
             * Implement your hack-logic right here!
             * Feel free to copy code from earlier excercises :)
             */
        }
    }
}
