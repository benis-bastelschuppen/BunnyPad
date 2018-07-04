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
        static ListBox output=null;
        static LinkedList<string> preList = new LinkedList<string>();

        public static void Line(string t)
        {
            Console.Out.WriteLine(t);
            try
            {
                output.Invoke((MethodInvoker)delegate
                {
                    // add the "lost data" list to the log system.
                    if(preList.Count>0)
                    {
                        foreach(string itm in preList)
                        {
                            output.Items.Add(itm);
                        }
                        preList.Clear();
                    }
                    // add the actual text to the log system.
                    output.Items.Add(t);
                    output.SelectedIndex = output.Items.Count - 1;
                    // deselect the item after scrolling.
                    output.SetSelected(output.Items.Count - 1, false);
                });
            }
            catch (Exception ex)
            {
                // prevent the log system from losing data.
                preList.AddLast(t);
            }
        }

        public static void Append(string txt)
        {
            try
            {
                output.Invoke((MethodInvoker)delegate
                {
                    // add the "lost data" list to the log system.
                    if (preList.Count > 0)
                    {
                        foreach (string itm in preList)
                        {
                            output.Items.Add(itm);
                        }
                        preList.Clear();
                    }
                    // add the actual text to the log system.

                    string txt2 = txt;
                    if (output.Items.Count > 0)
                    {
                        txt2 = output.Items[output.Items.Count - 1].ToString() + txt;
                        output.Items.RemoveAt(output.Items.Count - 1);
                    }

                    output.Items.Add(txt2);
                    output.SelectedIndex = output.Items.Count - 1;
                    // deselect the item after scrolling.
                    output.SetSelected(output.Items.Count - 1, false);
                });
            }
            catch (Exception ex)
            {
                if (preList.Count < 0)
                {
                    preList.AddLast(txt);
                }
                else
                {
                    string txt2 = preList.Last.ToString();
                    txt2 += txt;
                    preList.RemoveLast();
                    preList.AddLast(txt2);
                }
            }
        }

        public static void SetOutputListbox(ListBox lb) { output = lb; }
    }
}
