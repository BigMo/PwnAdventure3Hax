using SuperiorHackBase.Core;
using SuperiorHackBase.Core.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PwnAdventure3Hax.Hax
{
    public class PwnieHax02_NoClip : PwnieHax
    {
        private static Pointer POSITION_ADDRESS = new Pointer(0x00000000); //TODO: Figure out!

        protected override void OnKeyDown(Keys key)
        {
            base.OnKeyDown(key);

            switch (key)
            {
                case Keys.NumPad8:
                    MovePlayerBy(Vector3.UnitY * 100);
                    break;
                case Keys.NumPad2:
                    MovePlayerBy(Vector3.UnitY * -100);
                    break;
                case Keys.NumPad6:
                    MovePlayerBy(Vector3.UnitX * 100);
                    break;
                case Keys.NumPad4:
                    MovePlayerBy(Vector3.UnitX * -100);
                    break;
                case Keys.NumPad9:
                    MovePlayerBy(Vector3.UnitZ * 10000);
                    break;
                case Keys.NumPad3:
                    MovePlayerBy(Vector3.UnitZ * -50);
                    break;
            }
        }

        private void MovePlayerBy(Vector3 offset)
        {
            var position = this.Memory.Read<Vector3>(POSITION_ADDRESS);
            MovePlayerTo(position + offset);
        }
        private void MovePlayerTo(Vector3 position)
        {
            this.Memory.Write(POSITION_ADDRESS, position);
        }
    }
}
