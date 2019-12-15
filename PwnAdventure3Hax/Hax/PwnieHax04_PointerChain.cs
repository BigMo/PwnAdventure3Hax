using SuperiorHackBase.Core;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Graphics;
using SuperiorHackBase.Graphics.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwnAdventure3Hax.Hax
{
    public class PwnieHax04_PointerChain : PwnieHax
    {
        //private static Pointer[] POSITION_CHAIN = new Pointer[] { /* TODO: Figure out! */ };
        private static Pointer[] POSITION_CHAIN = new Pointer[] { 0x01900600 , 0x2C, 0x4C, 0x4, 0x1BC, 0x268, 0x114, 0x90 };

        private Vector3 currentPosition = Vector3.Zero;

        private Label positionLabel;
        private Button portSpell, portExit, portMajorPayne;

        public PwnieHax04_PointerChain()
        {
            var font = new FontDescription("Segoe UI", 14f);
            positionLabel = new Label()
            {
                Text = "Position: ",
                AutoSize = true,
                BackColor = BrushDescription.Transparent,
                ForeColor = BrushDescription.White,
                Location = new Vector2(20, 200),
                Font = font
            };
            portSpell = new Button()
            {
                Text = "TP to Spellbook",
                Size = new Vector2(140, 20),
                BackColor = BrushDescription.Black,
                ForeColor = BrushDescription.White,
                Location = new Vector2(20, 240),
                Font = font
            };
            portExit = new Button()
            {
                Text = "TP to Cave Exit",
                Size = new Vector2(140, 20),
                BackColor = BrushDescription.Black,
                ForeColor = BrushDescription.White,
                Location = new Vector2(20, 265),
                Font = font
            };
            portMajorPayne = new Button()
            {
                Text = "TP to Major Payne",
                Size = new Vector2(140, 20),
                BackColor = BrushDescription.Black,
                ForeColor = BrushDescription.White,
                Location = new Vector2(20, 290),
                Font = font
            };
            
            Overlay.RootControl.AddChild(positionLabel);
            Overlay.RootControl.AddChild(portSpell);
            Overlay.RootControl.AddChild(portExit);
            Overlay.RootControl.AddChild(portMajorPayne);

            portSpell.Pressed += (o, e) => Teleport(new Vector3(-43649.91f, -55970.77f, 323.69f));
            portExit.Pressed += (o, e) => Teleport(new Vector3(-53662.20f, -44927.45f, 375.11f));
            portMajorPayne.Pressed += (o, e) => Teleport(new Vector3(-37498.92f, -18424.39f, 2513.98f));
        }

        private void Teleport(Vector3 pos)
        {
            var positionAddress = this.Memory.ResolvePointerChain(MainModule.BaseAddress, POSITION_CHAIN); //Resolve the pointer chain
            this.Memory.Write(positionAddress, pos);
        }

        protected override void OnOverlayDrawing(RenderingEventArgs e)
        {
            base.OnOverlayDrawing(e);
        }

        protected override void OnTick(TimeSpan delta)
        {
            base.OnTick(delta);

            var positionAddress = this.Memory.ResolvePointerChain(MainModule.BaseAddress, POSITION_CHAIN); //Resolve the pointer chain
            var newPosition = this.Memory.Read<Vector3>(positionAddress); //Read the current position
            positionLabel.Text = $"Position: {(int)newPosition.X}, {(int)newPosition.Y}, {(int)newPosition.Z}";
            if (currentPosition != newPosition) //If the position changed...
            {
                currentPosition = newPosition; //... save its new value ...
                Console.WriteLine("{0}\t{1}\t{2}", //... and print it!
                    (int)currentPosition.X, (int)currentPosition.Y, (int)currentPosition.Z);
            }
        }
    }
}
