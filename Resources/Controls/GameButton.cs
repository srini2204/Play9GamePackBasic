using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Play9GamePackBasic.Resources.Controls
{
    public partial class GameButton : UserControl
    {

        [Category("Custom")]
        [Description("The game name displayed in bold")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string HeaderText
        {
            get
            {
                return lblHeader.Text;
            }
            set
            {
                lblHeader.Text = value;
                _gameData.HeaderText = value;
            }
        }

        [Category("Custom")]
        [Description("The client name displayed in small")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string DescText
        {
            get
            {
                return lblDesc.Text;
            }
            set
            {
                lblDesc.Text = value;
                _gameData.DescText = value;
            }
        }
        //Image.FromFile("BeigeMonitor1.png")
        [Category("Custom")]
        [Description("The button image")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public String ButtonImage
        {
            get
            {
                return _gameData.ImageSource;
            }
            set
            {
                _gameData.ImageSource = value;
                try
                {
                    if (value!= null)
                    {
                        pbButton.BackgroundImage = Image.FromFile(value); 
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        [Category("Custom")]
        [Description("The border image")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public String BorderImage
        {
            get
            {
                return _gameData.BordeColor;
            }
            set
            {
                _gameData.BordeColor = value;
                try
                {
                    if (value != null)
                    {
                        pbBorder.BackgroundImage = Image.FromFile(value);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public GameModel GameData
        {
            get
            {
                return _gameData;
            }
            set
            {
                _gameData = value;
                lblHeader.Text = _gameData.HeaderText;
                lblDesc.Text = _gameData.DescText;
                pbBorder.BackgroundImage = Image.FromFile(_gameData.BordeColor);
                pbButton.BackgroundImage = Image.FromFile(_gameData.ImageSource);
            }
        }
        private GameModel _gameData;

        public GameButton()
        {
            _gameData = new GameModel();
            InitializeComponent();
        }
    }
}
