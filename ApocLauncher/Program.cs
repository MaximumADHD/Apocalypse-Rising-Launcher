using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace ApocLauncher
{
    static class Program
    {
        [STAThread]

        static void Main()
        {
            Application.Run(new Launcher());
        }
    }
}
