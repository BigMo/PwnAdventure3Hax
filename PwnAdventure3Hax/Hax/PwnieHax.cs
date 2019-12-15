using SuperiorHackBase.Core;
using SuperiorHackBase.Core.Maths;
using SuperiorHackBase.Core.ProcessInteraction;
using SuperiorHackBase.Core.ProcessInteraction.Process;
using SuperiorHackBase.Graphics;
using SuperiorHackBase.Input;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PwnAdventure3Hax.Hax
{
    /// <summary>
    /// Base class for our Pwnie Island hacks!
    /// Terminates when [CTRL]+[Q] are pressed.
    /// </summary>
    public class PwnieHax : LocalHackContext
    {
        public const string PROCESS_NAME = "PwnAdventure3-Win32-Shipping";
        public const string MODULE_NAME = "PwnAdventure3-Win32-Shipping.exe";

        private KeyboardHook keyboard;
        private MouseHook mouse;
        private Vector2 mousePos = DEFAULT_POSITION;
        private static Vector2 DEFAULT_POSITION = new Vector2(-1, -1);
        protected GameOverlay Overlay { get; private set; }
        private SuperiorHackBase.Graphics.Controls.Label performanceLabel;
        private RoundRobinBuffer fps;

        public IModule MainModule => Process.FindModule(MODULE_NAME);
        protected float AverageFPS => fps.Average;

        public PwnieHax() : this(LocalProcess.Find(PROCESS_NAME)) { }
        private PwnieHax(LocalProcess process) : base(process, process.CreateMemoryInterface(WinAPI.ProcessAccessFlags.All, true))
        {
            //Initialize & set up low-level keyboard hook
            keyboard = new KeyboardHook();
            keyboard.Hook();
            keyboard.KeyUp += (o, e) => { OnKeyUp(e.Key); };
            keyboard.KeyDown += (o, e) => { OnKeyDown(e.Key); };
            mouse = new MouseHook();
            mouse.Hook();
            mouse.MouseEvent += Mouse_MouseEvent;
            //Initialize & set up overlay
            Overlay = new GameOverlay(this, "PwnieHax");
            Overlay.Drawing += Overlay_Drawing;
            Overlay.DrawOnlyWhenInForeground = false;
            Overlay.RegisterHooks(keyboard, mouse);
            MainForm = Overlay;
            //
            performanceLabel = new SuperiorHackBase.Graphics.Controls.Label()
            {
                Text = "- FPS\n- read\n- write",
                AutoSize = true,
                BackColor = BrushDescription.Transparent,
                ForeColor = BrushDescription.White,
                Location = new Vector2(4, 4),
                Font = new FontDescription("Courier New", 12f)
            };
            Overlay.RootControl.AddChild(performanceLabel);
            fps = new RoundRobinBuffer(60);
        }

        private void Mouse_MouseEvent(object sender, MouseEventExtArgs e)
        {
            if (e.UpOrDown == UpDown.Up) OnMouseUp(e.Button);
            if (e.UpOrDown == UpDown.Down) OnMouseDown(e.Button);
            var relPos = e.MakeRelative(Overlay.Location).Position;
            if (mousePos == DEFAULT_POSITION) mousePos = relPos;
            if (mousePos != relPos) OnMouseMoved(mousePos, relPos, relPos - mousePos);
            mousePos = relPos;
            if (e.WheelMoved && e.WheelDelta != 0) OnMouseWheelScrolled(e.WheelDelta);
        }

        protected override void OnTick(TimeSpan delta)
        {
            base.OnTick(delta);

        }

        protected virtual void OnMouseWheelScrolled(int wheelDelta) { }
        protected virtual void OnMouseDown(MouseButtons button) { }
        protected virtual void OnMouseUp(MouseButtons button) { }
        protected virtual void OnMouseMoved(Vector2 from, Vector2 to, Vector2 delta) { }

        private static string FormatBytes(double bytes)
        {
            int i = 0;
            for (i = 0; bytes >= 1000; i++, bytes /= 1000) ;
            return $"{bytes.ToString("0.0")} {new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" }[i]}";
        }

        private void Overlay_Drawing(object sender, RenderingEventArgs e)
        {
            fps.Add((float)(1.0 / e.Delta.TotalSeconds));
            performanceLabel.Text = string.Format(
                "[INFO]\n{0} FPS\n{1} read\n{2} write",
                AverageFPS.ToString("0.00"), FormatBytes(this.Memory.BytesRead), FormatBytes(this.Memory.BytesWrite));
            OnOverlayDrawing(e);
        }

        protected virtual void OnOverlayDrawing(RenderingEventArgs e)
        {

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            keyboard.Dispose(); //Make sure to dispose (and thus unhook) our keyboard hook!
        }

        protected virtual void OnKeyDown(Keys key)
        {
            if (key.HasFlag(Keys.Control) && key.HasFlag(Keys.Q)) Exit();
        }

        protected virtual void OnKeyUp(Keys key) { }

        private class RoundRobinBuffer
        {
            private float[] buffer;
            private int index;
            private int numValues;

            public RoundRobinBuffer(int length)
            {
                buffer = new float[length];
            }

            public void Add(float num)
            {
                buffer[index] = num;
                index = (index + 1) % buffer.Length;
                numValues++;
            }

            public float Average
            {
                get
                {
                    if (numValues == 0) return 0;
                    var n = Math.Min(numValues, buffer.Length);
                    return buffer.Take(n).Sum() / n;
                }
            }
        }
    }
}
