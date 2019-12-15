using PwnAdventure3Hax.Hax;
using SuperiorHackBase.Core.ProcessInteraction;
using SuperiorHackBase.Core.ProcessInteraction.Memory;
using System;
using System.Windows.Forms;

namespace PwnAdventure3Hax
{
    class Program
    {
        /// <summary>
        /// This is where our program starts executing.
        /// No touchy touchy in this method!
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                //Run the hack...
                RunHack(new PwnieHax02_NoClip());
            }
            catch (Exception ex)
            {
                //... until either an exception occurs ...
                Console.WriteLine(
                    "Something went terribly wrong and made your hack crash.\n" +
                    "The exceptions that were thrown will be shown below, you probably want to check the very LAST exception first.\n" +
                    "Here we go:\n"
                    );
                int depth = 0;
                do
                {
                    Console.WriteLine(
                        "===================================\n" +
                        "Exception #{0}: {1}\n" +
                        "Stacktrace:\n" +
                        "{2}",
                        ++depth,
                         ex.Message,
                         ex.StackTrace);
                    if (ex.GetType() == typeof(ReadWriteMemoryException))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(">>> Looks like you messed something up! The hack could not read or write some data. Check your addresses/offsets using CheatEngine!");
                        Console.ResetColor();
                    }
                } while ((ex = ex.InnerException) != null);

            }
            //... or the program is done executing.
            Console.WriteLine("Press [ENTER] to quit.");
            Console.ReadLine();
        }

        /// <summary>
        /// No touchy touchy in this method either!
        /// </summary>
        /// <param name="hax"></param>
        private static void RunHack(PwnieHax hax)
        {
            //TODO: Replace excercise with the current hack to use!
            Console.WriteLine("Starting hack: {0}", hax.GetType().Name);
            Application.Run(hax);
            Console.WriteLine("Hack exited.");
        }
    }
}
