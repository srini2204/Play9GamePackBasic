using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play9GamePackBasic
{
    public partial class Pay9MainForm : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public Pay9MainForm()
        {
            InitializeComponent();
            var GameData = new Play9GamePackBasic.Resources.Controls.GameModel()
            {
                BordeColor = "D:/workspace/Play9GamePackBasic/Play9GamePackBasic/Resources/Images/IconBackgrounds/Box Yellow.png",
                DescColor = "Black",
                DescFont = "Arial",
                DescSize = 18,
                DescText = "Pegasus",
                HeaderColor = "Black",
                HeaderFont = "Arial Bold",
                HeaderSize = 30,
                HeaderText = "Kids Kitchen",
                ImageSource = "D:/workspace/Play9GamePackBasic/Play9GamePackBasic/Resources/Images/GameIcons/10 KidsKitchen.png",
                GamePath = @"D:\workspace\of_v0.8.4_vs_release\apps\PegasusHighwaysHotelKidsZone\bin\PegasusHighwaysHotelLauncher\PegasusHighwaysHotelLauncher.exe"
            };
            customGameButton1.DataContext = GameData;
        }

        private void pbButton_Click(object sender, EventArgs e)
        {
            RunApp r = new RunApp();
            contentPath = @"D:\workspace\of_v0.8.4_vs_release\apps\PegasusHighwaysHotelKidsZone\bin\PegasusHighwaysHotelLauncher\PegasusHighwaysHotelLauncher.exe";
            ProcessStartInfo contentInfo = new ProcessStartInfo();

            contentInfo.FileName = contentPath;
            contentInfo.WorkingDirectory = Path.GetDirectoryName(contentPath);

            contentInfo.UseShellExecute = true;
            contentInfo.CreateNoWindow = true;
            contentInfo.WindowStyle = ProcessWindowStyle.Maximized;
            contentInfo.RedirectStandardInput = false;
            contentInfo.RedirectStandardOutput = false;
            contentInfo.RedirectStandardError = false;

            content = Process.Start(contentInfo);
            content.WaitForInputIdle();
            SetParent(content.MainWindowHandle, this.Handle);
            content.EnableRaisingEvents = true;
            content.Exited += Content_Exited;
            while (!content.Responding)
            {
                Game_Loaded();
            }
        }

        public Process content;
        private string contentPath;

        private void Game_Loaded()
        {

        }

        private void Content_Exited(object sender, EventArgs e)
        {

        }
    }
}
