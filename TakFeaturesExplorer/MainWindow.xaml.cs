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
using TakLib.AI;

namespace TakFeaturesExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game _game;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenGameFileExecute, OpenGameFileCanExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Find, ShowNextMovesExecute, ShowNextMovesCanExecute));
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Properties, ShowChartExecute, ShowChartCanExecute));
            BoardListView.SelectionChanged += BoardAnalysisListViewOnSelectionChanged;
        }

        private void ShowNextMovesCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _game != null && BoardListView.SelectedItem != null;
        }

        private void ShowChartExecute(object sender, ExecutedRoutedEventArgs e)
        {
            GameChartWindow chartWindow = new GameChartWindow(_game);
            chartWindow.Show();
        }

        private void ShowChartCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _game != null;
        }

        private void OpenGameFileExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this).Value)
            {
                BoardListView.ItemsSource = null;
                _game = Game.CreateGameFromTranscript(openFileDialog.FileName);
                if (_game == null)
                {
                    MessageBox.Show($"Error creating game from file {openFileDialog.FileName}");
                    return;
                }
                MaximumRatioAnalyzer analyzer = new MaximumRatioAnalyzer(_game.Boards[0].Size);
                BoardListView.ItemsSource = _game.Boards.Select(b => new Tuple<IAnalysisResult, Board>(analyzer.Analyze(b), b));
                if (_game.Boards != null && _game.Boards.Count > 0)
                {
                    BoardUserControl.Board = _game.Boards[0];
                }
            }
        }

        private void ShowNextMovesExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Board board = ((Tuple<IAnalysisResult, Board>)BoardListView.SelectedItem).Item2;
            NextMovesWindows movesWindow = new NextMovesWindows(board);
            movesWindow.Show();
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
                BoardUserControl.DrawBoard((MaximumRatioAnalysisData)((Tuple<IAnalysisResult, Board>)selectionChangedEventArgs.AddedItems[0]).Item1);
            }
        }

    }
}
