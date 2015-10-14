using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play9GamePackBasic.Resources.Controls
{
    public class GameModel
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

        String _headerFont;
        public String HeaderFont
        {
            get { return _headerFont; }
            set { _headerFont = value; }
        }

        String _descFont;
        public String DescFont
        {
            get { return _descFont; }
            set { _descFont = value; }
        }

        String _headerColor;
        public String HeaderColor
        {
            get { return _headerColor; }
            set { _headerColor = value; }
        }

        String _descColor;
        public String DescColor
        {
            get { return _descColor; }
            set { _descColor = value; }
        }

        int _headerSize;
        public int HeaderSize
        {
            get { return _headerSize; }
            set { _headerSize = value; }
        }

        int _descSize;
        public int DescSize
        {
            get { return _descSize; }
            set { _descSize = value; }
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
