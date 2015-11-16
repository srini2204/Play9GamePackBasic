using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play9GamePackBasic.Resources.Controls
{
    public class GameModel : GameModelMaster
    {
        String _headerText;
        public String HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        String _descText;
        public String DescText
        {
            get { return _descText; }
            set { _descText = value; }
        }

        String _imageSource;
        public String ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; }
        }

        String _bordeColor;
        public String BordeColor
        {
            get { return _bordeColor; }
            set { _bordeColor = value; }
        }

        string _gamePath;
        public string GamePath
        {
            get { return _gamePath; }
            set { _gamePath = value; }
        }
    }
}
