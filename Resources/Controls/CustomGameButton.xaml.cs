using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Play9GamePackBasic.Resources.Controls
{
    /// <summary>
    /// Interaction logic for CustomGameButton.xaml
    /// </summary>
    public partial class CustomGameButton : UserControl
    {

        /// <summary>
        /// Gets or sets the Value which is being displayed
        /// </summary>
        public GameModel GameData
        {
            get { return (GameModel)GetValue(GameDataProperty); }
            set { SetValue(GameDataProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty GameDataProperty =
            DependencyProperty.Register("GameData", typeof(GameModel),
              typeof(CustomGameButton), new PropertyMetadata(null));

        public CustomGameButton()
        {
            InitializeComponent();
            this.DataContext = GameData;
        }

        
    }
}
