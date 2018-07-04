using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Majestic_11
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Frm_MJOY_Main f = new Frm_MJOY_Main();
            Log.SetOutputListbox(f.LB_Console);
            Log.Line("Hello and Welcome to the joy of a mouse.");
            Log.Line("Use your gamepad as a mouse.");
            Log.Line(" ");
            Log.Line("based on JoyMouse from ben0bi in about 2005,");
            Log.Line("rewritten by the same ben0bi in 2018.");
            Log.Line("https://github.com/ben0bi");
            Log.Line("https://benedictinerklosterbruder.blogspot.com");
            Log.Line("write to jaeggiben at gmail dot com.");
            Log.Line(" ");
            Log.Line("Written in C# using Visual Studio Community Edition 2017.");
            Log.Line("Using SharpDX for the gamepad recognition.");
            Log.Line(" ");
            Log.Line("SharpDX and SharpDX.XInput by Alexandre Mutel");
            Log.Line("Version 4.1.0 - SharpDX is a DirectX wrapper for .NET applications.");
            Log.Line("http://sharpdx.org");
            Log.Line("------------------------------------------------------------------------------------------------------");

            XInputController controlpoller = new XInputController(f);
            Application.Run(f);

        }
    }
}
