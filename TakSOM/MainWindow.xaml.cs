using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
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
using TakLib;
using TakLib.AI.Helpers;

namespace TakSOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommonOpenFileDialog _openFolderDialog;
        List<Tuple<BoardAnalysisData, Board>> _boardsToAnalyze = new List<Tuple<BoardAnalysisData, Board>>();

        public MainWindow()
        {
            InitializeComponent();
            _openFolderDialog = new CommonOpenFileDialog("Open PTN Folder");
            _openFolderDialog.IsFolderPicker = true;
            _openFolderDialog.InitialDirectory = null;
            _openFolderDialog.AddToMostRecentlyUsedList = false;
            _openFolderDialog.AllowNonFileSystemItems = false;
            _openFolderDialog.EnsureFileExists = true;
            _openFolderDialog.EnsurePathExists = true;
            _openFolderDialog.EnsureReadOnly = false;
            _openFolderDialog.EnsureValidNames = true;
            _openFolderDialog.Multiselect = false;
            _openFolderDialog.ShowPlacesList = true;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenLatticeFileExecute, OpenLatticeFileCanExecute));
        }

        private void Analyze_OnClick(object sender, RoutedEventArgs e)
        {
            GameSetup gameSetup = new GameSetup()
            {
                WhitePlayer = new Player() { Name = "Player1" },
                BlackPlayer = new Player() { Name = "Player2" },
                BoardSize = 5
            };
            Game game = Game.GetNewGame(gameSetup);
            game.ApplyMove(NotationParser.Parse("a5"));
            game.ApplyMove(NotationParser.Parse("e1"));
            game.ApplyMove(NotationParser.Parse("b4"));
            game.ApplyMove(NotationParser.Parse("c2"));
            game.ApplyMove(NotationParser.Parse("d4"));
            game.ApplyMove(NotationParser.Parse("c3"));
            game.ApplyMove(NotationParser.Parse("a4"));
            game.ApplyMove(NotationParser.Parse("a5-"));
            game.ApplyMove(NotationParser.Parse("b4<"));
            game.ApplyMove(NotationParser.Parse("e3"));
            game.ApplyMove(NotationParser.Parse("2a4>"));
            game.ApplyMove(NotationParser.Parse("a2"));
            game.ApplyMove(NotationParser.Parse("Sb2"));
            game.ApplyMove(NotationParser.Parse("c5"));
            game.ApplyMove(NotationParser.Parse("e4"));
            game.ApplyMove(NotationParser.Parse("Sc4"));

            BoardAnalyzer analyzer = new BoardAnalyzer(game.CurrentBoard.Size, BoardAnalysisWeights.bestWeights);
            
            BoardDisplayTest boardDisplayTest = new BoardDisplayTest(game.Boards.ToList().Select(b => new Tuple<IAnalysisResult,Board>(analyzer.Analyze(b), b)).ToList());
            boardDisplayTest.Show();
        }

        private async void Train_OnClick(object sender, RoutedEventArgs e)
        {
            if (_boardsToAnalyze.Count == 0) return;
            BoardAnalyzer analyzer = new BoardAnalyzer(_boardsToAnalyze[0].Item2.Size, BoardAnalysisWeights.bestWeights);
            SOMWeightsVector v = analyzer.GetSomWeightsVector(_boardsToAnalyze[0].Item2);
            SOMTrainer trainer = new SOMTrainer();
            SOMLattice lattice = new SOMLattice(50, v.Count, analyzer);
            lattice.Initialize();
            List<SOMWeightsVector> weightsList = _boardsToAnalyze.Select(a => a.Item1.GetSomWeightsVector()).ToList();

            SOMAnalysisWindow analysisWindow = new SOMAnalysisWindow(lattice);
            Progress<int> progressReport = new Progress<int>((i) => ProgressBar1.Value = i);
            await Task.Run(() => trainer.Train(lattice, weightsList, progressReport, CancellationToken.None), CancellationToken.None);
            analysisWindow.CategorizeData(_boardsToAnalyze.Select(a => a.Item2));
            analysisWindow.RenderColors();
            analysisWindow.RenderCounts();
            analysisWindow.Show();
            SOMLattice.WriteLatticeData(lattice);

        }

        private void LoadFiles_OnClick(object sender, RoutedEventArgs e)
        {
            FileTextBox.Clear();
            _boardsToAnalyze.Clear();
            CommonFileDialogResult result = _openFolderDialog.ShowDialog();
            if(result == CommonFileDialogResult.Ok)
            {
                string directory = _openFolderDialog.FileName;

                IEnumerable<string> files = Directory.EnumerateFiles(directory, "*.ptn", SearchOption.TopDirectoryOnly);

                List<Game> games = new List<Game>();
                foreach (string file in files)
                {
                    try
                    {
                        games.Add(Game.CreateGameFromTranscript(file));
                        FileTextBox.Text += $"Loaded file {file}\n";
                    }
                    catch(Exception ex)
                    {
                        FileTextBox.Text += $"Error loading file {file}: {ex.Message}\n";
                    }
                }

                foreach(Game game in games)
                {
                    BoardAnalyzer analyzer = new BoardAnalyzer(game.CurrentBoard.Size, BoardAnalysisWeights.bestWeights);
                    _boardsToAnalyze.AddRange(game.Boards.Select(b => new Tuple<BoardAnalysisData, Board>((BoardAnalysisData)analyzer.Analyze(b), b)));
                }
            }
        }

        private void OpenLatticeFileExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog(this).Value)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (
                    Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read,
                        FileShare.None))
                {
                    var lattice = (SOMLattice)formatter.Deserialize(stream);
                    SOMAnalysisWindow analysisWindow = new SOMAnalysisWindow(lattice);
                    analysisWindow.RenderColors();
                    analysisWindow.RenderCounts();
                    analysisWindow.Show();
                }
            }
        }

        void OpenLatticeFileCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
