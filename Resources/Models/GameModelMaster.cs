using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play9GamePackBasic.Resources.Controls
{
    public class GameModelMaster
    {
        static String _headerFont;
        public static String HeaderFont
        {
            get { return _headerFont; }
            set { _headerFont = value; }
        }

        static String _descFont;
        public static String DescFont
        {
            get { return _descFont; }
            set { _descFont = value; }
        }

        static String _headerColor;
        public static String HeaderColor
        {
            get { return _headerColor; }
            set { _headerColor = value; }
        }

        static String _descColor;
        public static String DescColor
        {
            get { return _descColor; }
            set { _descColor = value; }
        }

        static int _headerSize;
        public static int HeaderSize
        {
            get { return _headerSize; }
            set { _headerSize = value; }
        }

        static int _descSize;
        public static int DescSize
        {
            get { return _descSize; }
            set { _descSize = value; }
        }
    }
}
