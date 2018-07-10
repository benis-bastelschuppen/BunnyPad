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
        static ListBox m_output=null;
        static LinkedList<string> m_loglist = new LinkedList<string>();

        public static void copyToListbox(ListBox outp)
        {
            if (outp == null)
                return;

            try
            {
                outp.Invoke((MethodInvoker)delegate
                {
                    outp.Items.Clear();
                    // add the "lost data" list to the log system.
                    if (m_loglist.Count > 0)
                    {
                        foreach (string itm in m_loglist)
                            outp.Items.Add(itm);
                        outp.SelectedIndex = outp.Items.Count - 1;
                        // deselect the item after scrolling.
                        outp.SetSelected(outp.Items.Count - 1, false);
                    }
                });
            }
            catch (Exception ex) { }
        }

        // output a single line in the log.
        public static void Line(string t)
        {
            Console.Out.WriteLine(t);
            m_loglist.AddLast(t);
            copyToListbox(m_output);
        }

        // append something to the last line of the log.
        public static void Append(string txt)
        {
            Console.Out.Write(txt);
            string t2 = txt;
            if(m_loglist.Count>0)
            {
                t2 = m_loglist.Last.ToString()+txt;
                m_loglist.RemoveLast();
            }
            m_loglist.AddLast(t2);
            copyToListbox(m_output);
        }

        public static void SetDefaultOutputListbox(ListBox lb) { m_output = lb; }
    }
}
