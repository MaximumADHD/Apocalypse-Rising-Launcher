using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace ApocLauncher
{
    public partial class Launcher : Form
    {
        int placeId = 1600503;
        WebClient http = new WebClient();

        public void DownloadRoblox(string path)
        {
            string filePath = Path.Combine(path, "RobloxPlayerLauncher.exe");
            if (!File.Exists(filePath))
            {
                byte[] Roblox = http.DownloadData("http://setup.roblox.com/Roblox.exe");
                FileStream download = File.Create(filePath);
                download.Write(Roblox, 0, Roblox.Length);
                download.Close();
            }
            Process player = Process.Start(filePath);
            player.WaitForExit();
            Process[] running = Process.GetProcessesByName("RobloxPlayerBeta");
            foreach (Process p in running)
            {
                try
                {
                    p.Kill();
                }
                catch
                {
                    // Sometimes it won't let us :/
                }
            }
        }

        public string getRobloxPath()
        {
            string[] envPaths = new string[] { 
                Environment.GetEnvironmentVariable("LocalAppData"), 
                Environment.GetEnvironmentVariable("ProgramFiles")
            };
            string version = http.DownloadString("http://setup.roblox.com/version");
            string exePath = null;
            foreach (string envPath in envPaths)
            {
                string directory = Path.Combine(envPath, "Roblox", "Versions", version, "RobloxPlayerBeta.exe");
                if (File.Exists(directory))
                {
                    exePath = directory;
                    break;
                }
            }
            if (exePath != null)
            {
                return exePath;
            }
            else
            {
                string appData = Environment.GetEnvironmentVariable("AppData");
                string root = Path.Combine(appData, "Roblox");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                DownloadRoblox(root);
                foreach (string envPath in envPaths)
                {
                    string directory = Path.Combine(envPath, "Roblox", "Versions", version, "RobloxPlayerBeta.exe");
                    if (File.Exists(directory))
                    {
                        return directory;
                    }
                }
            }
            throw new Exception("Couldn't locate RobloxPlayerBeta.exe!");
        }

        public Launcher()
        {
            InitializeComponent();
        }

        private async void Launcher_Load(object sender, EventArgs e)
        {
            // Hide the form using a bullshit method.
            this.Size = new Size(0, 0);
            this.Location = new Point(-1, -1);
            this.Visible = false;
            // Launch the game.
            await Task.Delay(1);
            ProcessStartInfo game = new ProcessStartInfo();
            game.FileName = getRobloxPath();
            game.UseShellExecute = true;
            game.Arguments = "--id " + placeId;
            Process roblox = Process.Start(game);
            // Wait for an exit and close.
            roblox.WaitForExit();
            Application.Exit();
        }
    }
}
