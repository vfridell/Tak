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
using System.Windows.Shapes;
using TakLib;
using TakWpfControls;

namespace TakFeaturesExplorer
{
    /// <summary>
    /// Interaction logic for GameChartWindow.xaml
    /// </summary>
    public partial class GameChartWindow : Window
    {
        public GameChartWindow(Game game)
        {
            InitializeComponent();
            FactorsGraph.Initialize(game);
        }
    }
}
