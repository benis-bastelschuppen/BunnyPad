using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Majestic_11
{
    public static class Log
    {
        static ListBox output;

        public static void Line(string t)
        {
            Console.Out.WriteLine(t);
            output.Items.Add(t);
        }

        public static void SetOutputListbox(ListBox lb) { output = lb; }
    }
}
