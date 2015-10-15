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
using Play9GamePackBasic.Resources.Controls;
using System.Xml;

namespace Play9GamePackBasic
{
    public partial class Pay9MainForm : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        LinkedList<GameModel> gameButtons;

        static Pay9MainForm()
        {
            GameModel.DescColor = "Black";
            GameModel.DescFont = "Arial";
            GameModel.DescSize = 18;
            GameModel.HeaderColor = "Black";
            GameModel.HeaderFont = "Arial Bold";
            GameModel.HeaderSize = 30;
        }

        public Pay9MainForm()
        {
            InitializeComponent();

            gameButtons = new LinkedList<GameModel>();
            loadGameButtonsData();
            setupGameButtons();

            var GameData = new GameModel()
            {
                BordeColor = "D:/workspace/Play9GamePackBasic/Play9GamePackBasic/Resources/Images/IconBackgrounds/Box Yellow.png",
                DescText = "Pegasus",
                HeaderText = "Kids Kitchen",
                ImageSource = "D:/workspace/Play9GamePackBasic/Play9GamePackBasic/Resources/Images/GameIcons/10 KidsKitchen.png",
                GamePath = @"D:\workspace\of_v0.8.4_vs_release\apps\PegasusHighwaysHotelKidsZone\bin\PegasusHighwaysHotelLauncher\PegasusHighwaysHotelLauncher.exe"
            };
            //gameButton1.GameData = GameData;
        }

        private void loadGameButtonsData()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "..\\..\\Resources\\Controls\\GameData.xml");

            XmlNodeList oXmlNodeList = xDoc.SelectNodes("AppData/HeaderText");
            foreach (XmlNode x in oXmlNodeList)
            {
                GameModel.HeaderColor = x.Attributes["FontColor"].Value;
                GameModel.HeaderFont = x.Attributes["FontFamily"].Value;
                GameModel.HeaderSize = Int32.Parse(x.Attributes["FontSize"].Value);
            }

            oXmlNodeList = xDoc.SelectNodes("AppData/DescText");
            foreach (XmlNode x in oXmlNodeList)
            {
                GameModel.DescColor = x.Attributes["FontColor"].Value;
                GameModel.DescFont = x.Attributes["FontFamily"].Value;
                GameModel.DescSize = Int32.Parse(x.Attributes["FontSize"].Value);
            }

            oXmlNodeList = xDoc.GetElementsByTagName("Game");
            GameModel gm;
            foreach (XmlNode x in oXmlNodeList)
            {
                gm = new GameModel();
                gm.HeaderText = x.ChildNodes[0].InnerText;
                gm.DescText = x.ChildNodes[1].InnerText;
                gm.ImageSource = x.ChildNodes[2].InnerText;
                gm.BordeColor = x.ChildNodes[3].InnerText;
                gm.GamePath = x.ChildNodes[4].InnerText;
                gameButtons.AddLast(gm);
            }
        }

        private void setupGameButtons()
        {
            for (int i = 0; i < 8; i++)
            {
                GameButton gb;
                foreach (var item in gameButtons)
                {
                    gb = new GameButton();
                    gb.HeaderText = item.HeaderText;
                    gb.DescText = item.DescText;
                    gb.BorderImage = item.BordeColor;
                    gb.ButtonImage = item.ImageSource;
                    gb.Margin = new Padding(45, 30, 0, 0);

                    buttonPanel.Controls.Add(gb);
                }
            }
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
            contentInfo.WindowStyle = ProcessWindowStyle.Normal;
            contentInfo.RedirectStandardInput = false;
            contentInfo.RedirectStandardOutput = false;
            contentInfo.RedirectStandardError = false;

            content = Process.Start(contentInfo);
            content.WaitForInputIdle();
            content.EnableRaisingEvents = true;
            content.Exited += Content_Exited;

            SetParent(content.MainWindowHandle, this.Handle);
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

        private void Pay9MainForm_Leave(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void Pay9MainForm_Enter(object sender, EventArgs e)
        {
            this.KeyPreview = false;
        }

        public void Pay9MainForm_Click(object sender, EventArgs e)
        {
            this.Capture = true;
        }
    }
}
