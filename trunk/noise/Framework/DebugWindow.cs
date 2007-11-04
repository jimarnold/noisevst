using System;
using System.Windows.Forms;

namespace Noise.Framework
{
    //You can use this puppy to log messages:
    //DebugWindow.Instance.Print("Oh hai!");
    //
    //Sorry about the singleton - I'll probably introduce some kind of logging interface later.
    //
    public partial class DebugWindow : Form
    {
        public static readonly DebugWindow Instance = new DebugWindow();

        public DebugWindow()
        {
            InitializeComponent();
        }

        public void Print(string message)
        {
            Invoke(
                (MethodInvoker)delegate
                {
                    textBox1.Text += Environment.NewLine + message;
                    textBox1.ScrollToCaret();
                }
            );
        }
    }
}