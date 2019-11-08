using SuperiorHackBase.Core;
using SuperiorHackBase.Core.ProcessInteraction;
using SuperiorHackBase.Core.ProcessInteraction.Process;
using SuperiorHackBase.Input;
using System;
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

        public IModule MainModule => Process.FindModule(MODULE_NAME);

        public PwnieHax() : this(LocalProcess.Find(PROCESS_NAME)) { }
        private PwnieHax(LocalProcess process) : base(process, process.CreateMemoryInterface(WinAPI.ProcessAccessFlags.All, true))
        {
            //Initialize & set up low-level keyboard hook
            keyboard = new KeyboardHook();
            keyboard.Hook();
            keyboard.KeyUp += (o, e) => { OnKeyUp(e.KeyCode); };
            keyboard.KeyDown += (o, e) => { OnKeyDown(e.KeyCode); };
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
    }
}
