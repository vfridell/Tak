using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
using Microsoft.Win32;
using TakLib;
using TakLib.AI.Helpers;

namespace TakFeaturesExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenGameFileExecute, OpenGameFileCanExecute));
        }

        private void OpenGameFileExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this).Value)
            {
                BoardListView.ItemsSource = null;
                Game game = Game.CreateGameFromTranscript(openFileDialog.FileName);
                if (game == null)
                {
                    MessageBox.Show($"Error creating game from file {openFileDialog.FileName}");
                    return;
                }
                BoardAnalyzer analyzer = new BoardAnalyzer(game.Boards[0].Size, BoardAnalysisWeights.bestWeights);
                BoardListView.ItemsSource = game.Boards.Select(b => new Tuple<IAnalysisResult, Board>(analyzer.Analyze(b), b));
                BoardListView.SelectionChanged += BoardAnalysisListViewOnSelectionChanged;
                if (game.Boards != null && game.Boards.Count > 0)
                {
                    BoardUserControl.Board = game.Boards[0];
                }

            }
        }

        private void OpenGameFileCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void BoardAnalysisListViewOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (selectionChangedEventArgs.AddedItems.Count > 0)
            {
                BoardUserControl.Board = ((Tuple<IAnalysisResult, Board>)selectionChangedEventArgs.AddedItems[0]).Item2;
                BoardUserControl.DrawBoard((BoardAnalysisData)((Tuple<IAnalysisResult, Board>)selectionChangedEventArgs.AddedItems[0]).Item1);
            }
        }
    }
}
